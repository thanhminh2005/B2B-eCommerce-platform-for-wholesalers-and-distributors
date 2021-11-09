using API.Domains;
using API.DTOs.Prices;
using API.DTOs.Products;
using API.DTOs.SubCategories;
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
                var distributor = await _unitOfWork.GetRepository<Distributor>().GetByIdAsync(Guid.Parse(request.DistributorId));
                if (distributor.IsActive)
                {
                    Product newProduct = _mapper.Map<Product>(request);
                    newProduct.Id = Guid.NewGuid();
                    newProduct.DistributorId = Guid.Parse(request.DistributorId);
                    newProduct.SubCategoryId = Guid.Parse(request.SubCategoryId);
                    newProduct.IsActive = true;
                    newProduct.DateCreated = DateTime.UtcNow;
                    newProduct.OrderTime = 0;
                    Price newPrice = new Price
                    {
                        Id = Guid.NewGuid(),
                        ProductId = newProduct.Id,
                        Value = request.Price,
                        Volume = request.MinQuantity,
                        DateCreated = DateTime.UtcNow
                    };
                    await _unitOfWork.GetRepository<Product>().AddAsync(newProduct);
                    await _unitOfWork.GetRepository<Price>().AddAsync(newPrice);
                    await _unitOfWork.SaveAsync();
                    return new Response<string>(newProduct.Name, message: "Product registered successfully ");
                }
                return new Response<string>(message: "Distributor is not activated ");
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
                var category = await _unitOfWork.GetRepository<SubCategory>().GetByIdAsync(product.SubCategoryId);
                var distributor = await _unitOfWork.GetRepository<Distributor>().GetByIdAsync(product.DistributorId);
                var user = await _unitOfWork.GetRepository<User>().GetByIdAsync(distributor.UserId);
                var listPrice = await _unitOfWork.GetRepository<Price>().GetAsync(filter: x => x.ProductId.Equals(product.Id),
                                                                             orderBy: x => x.OrderBy(y => y.Volume));
                List<PriceResponse> priceResponses = new List<PriceResponse>();
                foreach (var price in listPrice)
                {
                    PriceResponse CurPrice = _mapper.Map<PriceResponse>(price);
                    priceResponses.Add(CurPrice);
                }
                SubCategoryResponse CurSubCategory = _mapper.Map<SubCategoryResponse>(category);
                ProductResponse CurProduct = _mapper.Map<ProductResponse>(product);
                var parent = await _unitOfWork.GetRepository<Category>().GetByIdAsync(category.CategoryId);
                CurProduct.ParentCategoryId = parent.Id;
                CurProduct.ParentCategoryName = parent.Name;
                CurProduct.SubCategory = CurSubCategory;
                CurProduct.Distributor = user.Username;
                CurProduct.ListPrice = priceResponses;
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

                var category = await _unitOfWork.GetRepository<SubCategory>().GetByIdAsync(product.SubCategoryId);
                var distributor = await _unitOfWork.GetRepository<Distributor>().GetByIdAsync(product.DistributorId);
                var user = await _unitOfWork.GetRepository<User>().GetByIdAsync(distributor.UserId);
                var listPrice = await _unitOfWork.GetRepository<Price>().GetAsync(filter: x => x.ProductId.Equals(product.Id),
                                                                             orderBy: x => x.OrderBy(y => y.Volume));
                List<PriceResponse> priceResponses = new List<PriceResponse>();
                foreach (var price in listPrice)
                {
                    PriceResponse CurPrice = _mapper.Map<PriceResponse>(price);
                    priceResponses.Add(CurPrice);
                }
                SubCategoryResponse CurSubCategory = _mapper.Map<SubCategoryResponse>(category);
                RetailerGetProductsResponse CurProduct = _mapper.Map<RetailerGetProductsResponse>(product);
                var parent = await _unitOfWork.GetRepository<Category>().GetByIdAsync(category.CategoryId);
                CurProduct.ParentCategoryId = parent.Id;
                CurProduct.ParentCategoryName = parent.Name;
                CurProduct.SubCategory = CurSubCategory;
                CurProduct.Distributor = user.Username;
                CurProduct.ListPrice = priceResponses;
                response.Add(CurProduct);
            }
            return new PagedResponse<IEnumerable<RetailerGetProductsResponse>>(response, request.PageNumber, request.PageSize, totalcount);
        }

        public async Task<Response<ProductResponse>> GetProductById(GetProductByIdRequest request)
        {
            if (!string.IsNullOrWhiteSpace(request.Id))
            {
                var product = await _unitOfWork.GetRepository<Product>().GetByIdAsync(Guid.Parse(request.Id));
                if (product != null && product.IsActive)
                {
                    var category = await _unitOfWork.GetRepository<SubCategory>().GetByIdAsync(product.SubCategoryId);
                    var distributor = await _unitOfWork.GetRepository<Distributor>().GetByIdAsync(product.DistributorId);
                    var user = await _unitOfWork.GetRepository<User>().GetByIdAsync(distributor.UserId);
                    var listPrice = await _unitOfWork.GetRepository<Price>().GetAsync(filter: x => x.ProductId.Equals(product.Id),
                                                                             orderBy: x => x.OrderBy(y => y.Volume));
                    List<PriceResponse> priceResponses = new List<PriceResponse>();
                    foreach (var price in listPrice)
                    {
                        PriceResponse CurPrice = _mapper.Map<PriceResponse>(price);
                        priceResponses.Add(CurPrice);
                    }
                    SubCategoryResponse CurSubCategory = _mapper.Map<SubCategoryResponse>(category);
                    ProductResponse CurProduct = _mapper.Map<ProductResponse>(product);
                    var parent = await _unitOfWork.GetRepository<Category>().GetByIdAsync(category.CategoryId);
                    CurProduct.ParentCategoryId = parent.Id;
                    CurProduct.ParentCategoryName = parent.Name;
                    CurProduct.SubCategory = CurSubCategory;
                    CurProduct.Distributor = user.Username;
                    CurProduct.ListPrice = priceResponses;
                    return new Response<ProductResponse>(CurProduct, message: "Succeed");
                }
                else if (product != null && !product.IsActive)
                {
                    return new Response<ProductResponse>(message: "Product is removed");
                }
            }
            return new Response<ProductResponse>("Failed");
        }

        public async Task<PagedResponse<IEnumerable<RetailerGetProductsResponse>>> GetProductsWithFilter(GetProductsWithFilterRequest request)
        {
            IEnumerable<Product> products = null;
            int totalcount = 0;
            if (!string.IsNullOrWhiteSpace(request.CategoryId) && string.IsNullOrWhiteSpace(request.SubCategoryId))
            {
                var subCategories = await _unitOfWork.GetRepository<SubCategory>().GetAsync(x => x.CategoryId.Equals(Guid.Parse(request.CategoryId)));
                products = await _unitOfWork.GetRepository<Product>().GetPagedReponseAsync(request.PageNumber,
                                                                                     request.PageSize,
                                                                                     filter: x =>
                                                                                     (request.CategoryId == null || subCategories.Select(x => x.Id).Contains(x.SubCategoryId))
                                                                                     && (request.SearchValue == null || x.Name.Contains(request.SearchValue))
                                                                                     && (request.DistributorId == null || x.DistributorId.Equals(Guid.Parse(request.DistributorId)))
                                                                                     && (request.Status == 0 || x.Status.Equals(request.Status)),
                                                                                     orderBy: x => x.OrderBy(y => y.Name));
                totalcount = await _unitOfWork.GetRepository<Product>().CountAsync(filter: x =>
                                                                                      (request.CategoryId == null || subCategories.Select(x => x.Id).Contains(x.SubCategoryId))
                                                                                      && (request.SearchValue == null || x.Name.Contains(request.SearchValue))
                                                                                      && (request.DistributorId == null || x.DistributorId.Equals(Guid.Parse(request.DistributorId)))
                                                                                      && (request.Status == 0 || x.Status.Equals(request.Status)));
            }
            if (string.IsNullOrWhiteSpace(request.CategoryId) && !string.IsNullOrWhiteSpace(request.SubCategoryId))
            {
                products = await _unitOfWork.GetRepository<Product>().GetPagedReponseAsync(request.PageNumber,
                                                                                     request.PageSize,
                                                                                     filter: x =>
                                                                                     (request.SubCategoryId == null || x.SubCategoryId.Equals(Guid.Parse(request.SubCategoryId)))
                                                                                     && (request.SearchValue == null || x.Name.Contains(request.SearchValue))
                                                                                     && (request.DistributorId == null || x.DistributorId.Equals(Guid.Parse(request.DistributorId)))
                                                                                     && (request.Status == 0 || x.Status.Equals(request.Status)),
                                                                                     orderBy: x => x.OrderBy(y => y.Name));
                totalcount = await _unitOfWork.GetRepository<Product>().CountAsync(filter: x =>
                                                                                      (request.SubCategoryId == null || x.SubCategoryId.Equals(Guid.Parse(request.SubCategoryId)))
                                                                                      && (request.SearchValue == null || x.Name.Contains(request.SearchValue))
                                                                                      && (request.DistributorId == null || x.DistributorId.Equals(Guid.Parse(request.DistributorId)))
                                                                                      && (request.Status == 0 || x.Status.Equals(request.Status)));
            }
            List<RetailerGetProductsResponse> response = new List<RetailerGetProductsResponse>();
            if (products.Any())
            {
                foreach (var product in products)
                {
                    var category = await _unitOfWork.GetRepository<SubCategory>().GetByIdAsync(product.SubCategoryId);
                    var distributor = await _unitOfWork.GetRepository<Distributor>().GetByIdAsync(product.DistributorId);
                    var user = await _unitOfWork.GetRepository<User>().GetByIdAsync(distributor.UserId);
                    var listPrice = await _unitOfWork.GetRepository<Price>().GetAsync(filter: x => x.ProductId.Equals(product.Id),
                                                                                 orderBy: x => x.OrderBy(y => y.Volume));
                    List<PriceResponse> priceResponses = new List<PriceResponse>();
                    foreach (var price in listPrice)
                    {
                        PriceResponse CurPrice = _mapper.Map<PriceResponse>(price);
                        priceResponses.Add(CurPrice);
                    }
                    SubCategoryResponse CurSubCategory = _mapper.Map<SubCategoryResponse>(category);
                    RetailerGetProductsResponse CurProduct = _mapper.Map<RetailerGetProductsResponse>(product);
                    var parent = await _unitOfWork.GetRepository<Category>().GetByIdAsync(category.CategoryId);
                    CurProduct.ParentCategoryId = parent.Id;
                    CurProduct.ParentCategoryName = parent.Name;
                    CurProduct.SubCategory = CurSubCategory;
                    CurProduct.Distributor = user.Username;
                    CurProduct.ListPrice = priceResponses;
                    response.Add(CurProduct);
                }
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
                        NewProduct.SubCategoryId = Guid.Parse(request.SubCategoryId);
                        NewProduct.Name = request.Name;
                        NewProduct.Image = request.Image;
                        NewProduct.Status = request.Status;
                        NewProduct.Description = request.Description;
                        NewProduct.MinQuantity = request.MinQuantity;
                        NewProduct.IsActive = request.IsActive;
                        NewProduct.DateModified = DateTime.UtcNow;
                        _unitOfWork.GetRepository<Product>().UpdateAsync(NewProduct);
                        await _unitOfWork.SaveAsync();
                        return new Response<string>(NewProduct.Id.ToString(), message: "Product is updated");
                    }
                    return new Response<string>(message: "Product ID is not existed");
                }
                return new Response<string>(message: "Product ID can not be blanked");
            }
            return new Response<string>(message: "Fail to update product");
        }
        //get 30 random product and sort by most OrderTime
        public async Task<PagedResponse<IEnumerable<RetailerGetProductsResponse>>> GetProductsRecommendation()
        {
            List<Product> products = (List<Product>)await _unitOfWork.GetRepository<Product>().GetAllAsync();
            products = products.GetRandomItems(30);
            products.OrderByDescending(x => x.OrderTime);
            int totalcount = await _unitOfWork.GetRepository<Product>().CountAsync();
            List<RetailerGetProductsResponse> response = new List<RetailerGetProductsResponse>();
            foreach (var product in products)
            {
                var category = await _unitOfWork.GetRepository<SubCategory>().GetByIdAsync(product.SubCategoryId);
                var distributor = await _unitOfWork.GetRepository<Distributor>().GetByIdAsync(product.DistributorId);
                var user = await _unitOfWork.GetRepository<User>().GetByIdAsync(distributor.UserId);
                var listPrice = await _unitOfWork.GetRepository<Price>().GetAsync(filter: x => x.ProductId.Equals(product.Id),
                                                                             orderBy: x => x.OrderBy(y => y.Volume));
                List<PriceResponse> priceResponses = new List<PriceResponse>();
                foreach (var price in listPrice)
                {
                    PriceResponse CurPrice = _mapper.Map<PriceResponse>(price);
                    priceResponses.Add(CurPrice);
                }
                SubCategoryResponse CurSubCategory = _mapper.Map<SubCategoryResponse>(category);
                RetailerGetProductsResponse CurProduct = _mapper.Map<RetailerGetProductsResponse>(product);
                var parent = await _unitOfWork.GetRepository<Category>().GetByIdAsync(category.CategoryId);
                CurProduct.ParentCategoryId = parent.Id;
                CurProduct.ParentCategoryName = parent.Name;
                CurProduct.SubCategory = CurSubCategory;
                CurProduct.Distributor = user.Username;
                CurProduct.ListPrice = priceResponses;
                response.Add(CurProduct);
            }
            //get all in list, count all, get 10 random number not duplicate, add in list
            return new PagedResponse<IEnumerable<RetailerGetProductsResponse>>(response, 1, 30, totalcount);
        }
        //get all products like database order
        public async Task<PagedResponse<IEnumerable<ProductResponse>>> GetAll()
        {
            var products = await _unitOfWork.GetRepository<Product>().GetAllAsync();
            int totalcount = await _unitOfWork.GetRepository<Product>().CountAsync();
            List<ProductResponse> response = new List<ProductResponse>();
            foreach (var product in products)
            {
                var category = await _unitOfWork.GetRepository<SubCategory>().GetByIdAsync(product.SubCategoryId);
                var distributor = await _unitOfWork.GetRepository<Distributor>().GetByIdAsync(product.DistributorId);
                var user = await _unitOfWork.GetRepository<User>().GetByIdAsync(distributor.UserId);
                var listPrice = await _unitOfWork.GetRepository<Price>().GetAsync(filter: x => x.ProductId.Equals(product.Id),
                                                                             orderBy: x => x.OrderBy(y => y.Volume));
                List<PriceResponse> priceResponses = new List<PriceResponse>();
                foreach (var price in listPrice)
                {
                    PriceResponse CurPrice = _mapper.Map<PriceResponse>(price);
                    priceResponses.Add(CurPrice);
                }
                SubCategoryResponse CurSubCategory = _mapper.Map<SubCategoryResponse>(category);
                ProductResponse CurProduct = _mapper.Map<ProductResponse>(product);
                var parent = await _unitOfWork.GetRepository<Category>().GetByIdAsync(category.CategoryId);
                CurProduct.ParentCategoryId = parent.Id;
                CurProduct.ParentCategoryName = parent.Name;
                CurProduct.SubCategory = CurSubCategory;
                CurProduct.Distributor = user.Username;
                CurProduct.ListPrice = priceResponses;
                response.Add(CurProduct);
            }
            return new PagedResponse<IEnumerable<ProductResponse>>(response, 1, 30, totalcount);
        }
        //get list products by parent category
        public Task<PagedResponse<IEnumerable<RetailerGetProductsResponse>>> GetProductsByCategory(GetProductByCategoryIdRequest request)
        {
            throw new NotImplementedException();
        }
        //get list products by 1 subcategory
        public Task<PagedResponse<IEnumerable<RetailerGetProductsResponse>>> GetPopularProductsBySubCategory(GetProductByCategoryIdRequest request)
        {
            throw new NotImplementedException();
        }

        public async Task<Response<IEnumerable<ProductResponse>>> GetAllProductByDistributorId(GetProductByDistributorIdRequest request)
        {
            var products = await _unitOfWork.GetRepository<Product>().GetAsync(x => x.DistributorId.Equals(Guid.Parse(request.DistributorId)),
                                                                                      orderBy: x => x.OrderBy(y => y.Name));
            if (products != null)
            {
                List<ProductResponse> response = new List<ProductResponse>();
                foreach (var product in products)
                {
                    var category = await _unitOfWork.GetRepository<SubCategory>().GetByIdAsync(product.SubCategoryId);
                    var distributor = await _unitOfWork.GetRepository<Distributor>().GetByIdAsync(product.DistributorId);
                    var user = await _unitOfWork.GetRepository<User>().GetByIdAsync(distributor.UserId);
                    var listPrice = await _unitOfWork.GetRepository<Price>().GetAsync(filter: x => x.ProductId.Equals(product.Id),
                                                                                 orderBy: x => x.OrderBy(y => y.Volume));
                    List<PriceResponse> priceResponses = new List<PriceResponse>();
                    foreach (var price in listPrice)
                    {
                        PriceResponse CurPrice = _mapper.Map<PriceResponse>(price);
                        priceResponses.Add(CurPrice);
                    }
                    SubCategoryResponse CurSubCategory = _mapper.Map<SubCategoryResponse>(category);
                    ProductResponse CurProduct = _mapper.Map<ProductResponse>(product);
                    var parent = await _unitOfWork.GetRepository<Category>().GetByIdAsync(category.CategoryId);
                    CurProduct.ParentCategoryId = parent.Id;
                    CurProduct.ParentCategoryName = parent.Name;
                    CurProduct.SubCategory = CurSubCategory;
                    CurProduct.Distributor = user.Username;
                    CurProduct.ListPrice = priceResponses;
                    response.Add(CurProduct);
                }
                return new Response<IEnumerable<ProductResponse>>(response, message: "Succeed");
            }
            return new Response<IEnumerable<ProductResponse>>("Not Found");
        }
    }
}
