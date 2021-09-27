using API.Domains;
using API.DTOs.Products;
using API.Interfaces;
using API.Warppers;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
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

        //get products list from a specific distributorID
        public async Task<Response<IEnumerable<ProductResponse>>> GetProductByDistributorId(GetProductByDistributorIdRequest request)
        {
            var products = await _unitOfWork.GetRepository<Product>().GetAsync(x => x.DistributorId.Equals(Guid.Parse(request.DistributorId)));
            if (products.Count() != 0)
            {
                return new Response<IEnumerable<ProductResponse>>(_mapper.Map<IEnumerable<ProductResponse>>(products), message: "Success");
            }
            return new Response<IEnumerable<ProductResponse>>(message: "Empty");
        }

        public async Task<Response<ProductResponse>> GetProductById(GetProductByIdRequest request)
        {
            if (!string.IsNullOrWhiteSpace(request.Id))
            {
                var product = await _unitOfWork.GetRepository<Product>().GetByIdAsync(Guid.Parse(request.Id));
                if (product != null && product.IsActive != false)
                {
                    return new Response<ProductResponse>(_mapper.Map<ProductResponse>(product), message: "Succeed");
                }
                else if(!product.IsActive)
                {
                    return new Response<ProductResponse>(message: "Product is removed");
                }
            }
            return new Response<ProductResponse>("Failed");
        }

        public async Task<PagedResponse<IEnumerable<ProductResponse>>> GetProductsWithFilter(GetProductsWithFilterRequest request)
        {
            var products = await _unitOfWork.GetRepository<Product>().GetPagedReponseAsync(request.PageNumber,
                                                                                      request.PageSize,
                                                                                      filter: x => (request.CategoryId == null || x.CategoryId.Equals(Guid.Parse(request.CategoryId))
                                                                                      && (request.SearchValue == null || x.Name.Contains(request.SearchValue))
                                                                                      && (request.DistributorId == null || x.DistributorId.Equals(Guid.Parse(request.DistributorId)))
                                                                                      && (request.Status == 0 || x.Status.Equals(request.Status))
                                                                                      ));

            var totalcount = await _unitOfWork.GetRepository<Product>().CountAsync(filter: x => (request.CategoryId == null || x.CategoryId.Equals(Guid.Parse(request.CategoryId))
                                                                                      && (request.SearchValue == null || x.Name.Contains(request.SearchValue))
                                                                                      && (request.DistributorId == null || x.DistributorId.Equals(Guid.Parse(request.DistributorId)))
                                                                                      && (request.Status == 0 || x.Status.Equals(request.Status))
                                                                                      ));
            var response = _mapper.Map<IEnumerable<ProductResponse>>(products);
            return new PagedResponse<IEnumerable<ProductResponse>>(response, request.PageNumber, request.PageSize, totalcount);
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
                    if(!product.IsActive)
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
            
            if(request != null)
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


    }
}
