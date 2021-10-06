using API.Domains;
using API.DTOs.Categories;
using API.DTOs.Products;
using API.Helpers;
using API.Interfaces;
using API.Warppers;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Services
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<Response<string>> CreateProduct(CreateProductRequest request)
        {
            if (request != null)
            {
                var product = await _unitOfWork.GetRepository<Product>().FirstAsync(x => x.Name.Equals(request.Name));
                if (product == null)
                {
                    var distributor = await _unitOfWork.GetRepository<Distributor>().GetByIdAsync(Guid.Parse(request.DistributorId));

                    if (distributor.IsActive)
                    {
                        Product newProduct = _mapper.Map<Product>(request);
                        newProduct.Id = Guid.NewGuid();
                        newProduct.DistributorId = Guid.Parse(request.DistributorId);
                        newProduct.CategoryId = Guid.Parse(request.CategoryId);
                        newProduct.IsActive = true;
                        newProduct.DateCreated = DateTime.UtcNow;
                        newProduct.OrderTime = 0;
                        await _unitOfWork.GetRepository<Product>().AddAsync(newProduct);
                        await _unitOfWork.SaveAsync();
                        return new Response<string>(newProduct.Name, message: "Product registered successfully ");
                    }
                    return new Response<string>(message: "Distributor is removed ");
                }
                return new Response<string>(message: "Product's name is existed");
            }
            return new Response<string>(message: "Failed to create product");
        }

        //get products list from a specific distributorID (View by Distributor)
        public async Task<PagedResponse<IEnumerable<ProductResponse>>> GetProductByDistributorId(GetProductByDistributorIdRequest request)
        {
            var products = await _unitOfWork.GetRepository<Product>().GetPagedReponseAsync(request.PageNumber,
                                                                                      request.PageSize,
                                                                                      filter: x => x.DistributorId.Equals(Guid.Parse(request.DistributorId)),
                                                                                      orderBy: x => x.OrderBy(y => y.Name));

            var totalcount = await _unitOfWork.GetRepository<Product>().CountAsync(filter: x => x.DistributorId.Equals(Guid.Parse(request.DistributorId)));

            List<ProductResponse> response = new List<ProductResponse>();
            foreach (var product in products)
            {
                var category = await _unitOfWork.GetRepository<Category>().GetByIdAsync(product.CategoryId);
                CategoryResponse CurCategory = _mapper.Map<CategoryResponse>(category);
                ProductResponse CurProduct = _mapper.Map<ProductResponse>(product);
                CurProduct.Category = CurCategory;
                response.Add(CurProduct);
            }

            return new PagedResponse<IEnumerable<ProductResponse>>(response, request.PageNumber, request.PageSize, totalcount);
        }
        //View by Retailer
        public async Task<PagedResponse<IEnumerable<RetailerGetProductsResponse>>> RetailerGetProductByDistributorId(GetProductByDistributorIdRequest request)
        {
            var products = await _unitOfWork.GetRepository<Product>().GetPagedReponseAsync(request.PageNumber,
                                                                                      request.PageSize,
                                                                                      filter: x => x.DistributorId.Equals(Guid.Parse(request.DistributorId)),
                                                                                      orderBy: x => x.OrderBy(y => y.Name));
            
            var totalcount = await _unitOfWork.GetRepository<Product>().CountAsync(filter: x => x.DistributorId.Equals(Guid.Parse(request.DistributorId)));
           
            List<RetailerGetProductsResponse> response = new List<RetailerGetProductsResponse>();
            foreach (var product in products)
            {
                var category = await _unitOfWork.GetRepository<Category>().GetByIdAsync(product.CategoryId);
                CategoryResponse CurCategory = _mapper.Map<CategoryResponse>(category);
                RetailerGetProductsResponse CurProduct = _mapper.Map<RetailerGetProductsResponse>(product);
                CurProduct.Category = CurCategory;
                response.Add(CurProduct);
            }
            return new PagedResponse<IEnumerable<RetailerGetProductsResponse>>(response, request.PageNumber, request.PageSize, totalcount);
        }

        public async Task<Response<ProductResponse>> GetProductById(GetProductByIdRequest request)
        {
            if (!string.IsNullOrWhiteSpace(request.Id))
            {
                var product = await _unitOfWork.GetRepository<Product>().GetByIdAsync(Guid.Parse(request.Id));
                if (product != null && product.IsActive != false)
                {
                    var category = await _unitOfWork.GetRepository<Category>().GetByIdAsync(product.CategoryId);
                    CategoryResponse CurCategory = _mapper.Map<CategoryResponse>(category);
                    ProductResponse CurProduct = _mapper.Map<ProductResponse>(product);
                    CurProduct.Category = CurCategory;
                    return new Response<ProductResponse>(CurProduct, message: "Succeed");
                }
                else if (!product.IsActive)
                {
                    return new Response<ProductResponse>(message: "Product is removed");
                }
            }
            return new Response<ProductResponse>("Failed");
        }

        public async Task<PagedResponse<IEnumerable<RetailerGetProductsResponse>>> GetProductsWithFilter(GetProductsWithFilterRequest request)
        {
            var products = await _unitOfWork.GetRepository<Product>().GetPagedReponseAsync(request.PageNumber,
                                                                                      request.PageSize,
                                                                                      filter: x =>
                                                                                      (request.CategoryId == null || x.CategoryId.Equals(Guid.Parse(request.CategoryId)))
                                                                                      && (request.SearchValue == null || x.Name.Contains(request.SearchValue))
                                                                                      && (request.DistributorId == null || x.DistributorId.Equals(Guid.Parse(request.DistributorId)))
                                                                                      && (request.Status == 0 || x.Status.Equals(request.Status)),
                                                                                      orderBy: x => x.OrderBy(y => y.Name));

            var totalcount = await _unitOfWork.GetRepository<Product>().CountAsync(filter: x =>
                                                                                      (request.CategoryId == null || x.CategoryId.Equals(Guid.Parse(request.CategoryId)))
                                                                                      && (request.SearchValue == null || x.Name.Contains(request.SearchValue))
                                                                                      && (request.DistributorId == null || x.DistributorId.Equals(Guid.Parse(request.DistributorId)))
                                                                                      && (request.Status == 0 || x.Status.Equals(request.Status)));
            
            List<RetailerGetProductsResponse> response = new List<RetailerGetProductsResponse>();
            foreach (var product in products)
            {
                var category = await _unitOfWork.GetRepository<Category>().GetByIdAsync(product.CategoryId);
                CategoryResponse CurCategory = _mapper.Map<CategoryResponse>(category);
                RetailerGetProductsResponse CurProduct = _mapper.Map<RetailerGetProductsResponse>(product);
                CurProduct.Category = CurCategory;
                response.Add(CurProduct);
            }

            return new PagedResponse<IEnumerable<RetailerGetProductsResponse>>(response, request.PageNumber, request.PageSize, totalcount);
        }

        public async Task<Response<string>> RemoveProduct(RemoveProductRequest request)
        {
            if (!string.IsNullOrWhiteSpace(request.Id))
            {
                var product = await _unitOfWork.GetRepository<Product>().FirstAsync(x => x.Id.Equals(Guid.Parse(request.Id)));
                if (product != null)
                {
                    product.IsActive = request.IsActive;
                    product.DateModified = DateTime.UtcNow;
                    _unitOfWork.GetRepository<Product>().UpdateAsync(product);
                    await _unitOfWork.SaveAsync();
                    if (!product.IsActive)
                    {
                        return new Response<string>(product.Id.ToString(), message: "Product is removed");
                    }
                    else
                    {
                        return new Response<string>(product.Id.ToString(), message: "Product is activated");
                    }

                }
            }
            return new Response<string>(message: "Fail to remove product");
        }

        public async Task<Response<string>> UpdateProduct(UpdateProductRequest request)
        {

            if (request != null)
            {
                if (!string.IsNullOrWhiteSpace(request.Id))
                {
                    Product NewProduct = await _unitOfWork.GetRepository<Product>().FirstAsync(x => x.Id.Equals(Guid.Parse(request.Id)));

                    if (NewProduct != null)
                    {
                        NewProduct.CategoryId = Guid.Parse(request.CategoryId);
                        NewProduct.Name = request.Name;
                        NewProduct.Image = request.Image;
                        NewProduct.Status = request.Status;
                        NewProduct.Description = request.Description;
                        NewProduct.MinQuantity = request.MinQuantity;
                        NewProduct.DateModified = DateTime.UtcNow;
                        _unitOfWork.GetRepository<Product>().UpdateAsync(NewProduct);
                        await _unitOfWork.SaveAsync();
                        return new Response<string>(NewProduct.Id.ToString(), message: "Product is updated");
                    }
                }
                return new Response<string>(message: "Product ID can not be blanked");
            }
            return new Response<string>(message: "Fail to update product");
        }

        public async Task<PagedResponse<IEnumerable<RetailerGetProductsResponse>>> GetProductsRecommendation()
        {
            List<Product> products = (List<Product>)await _unitOfWork.GetRepository<Product>().GetAllAsync();
            products = products.GetRandomItems(30);
            int totalcount = await _unitOfWork.GetRepository<Product>().CountAsync();
            
            
            List<RetailerGetProductsResponse> response = new List<RetailerGetProductsResponse>();
            foreach (var product in products)
            {
                var category = await _unitOfWork.GetRepository<Category>().GetByIdAsync(product.CategoryId);
                CategoryResponse CurCategory = _mapper.Map<CategoryResponse>(category);
                RetailerGetProductsResponse CurProduct = _mapper.Map<RetailerGetProductsResponse>(product);
                CurProduct.Category = CurCategory;
                response.Add(CurProduct);
            }
            //get all in list, count all, get 10 random number not duplicate, add in list
            return new PagedResponse<IEnumerable<RetailerGetProductsResponse>>(response, 1, 30, totalcount);
        }

    }
}
