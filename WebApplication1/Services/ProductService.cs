using API.Domains;
using API.DTOs.Products;
using API.Interfaces;
using API.Warppers;
using AutoMapper;
using System;
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

        public async Task<Response<ProductResponse>> GetProductById(GetProductByIdRequest request)
        {
            if (!string.IsNullOrWhiteSpace(request.Id))
            {
                var product = await _unitOfWork.GetRepository<Product>().GetByIdAsync(Guid.Parse(request.Id));
                if (product != null && product.IsActive != false)
                {
                    return new Response<ProductResponse>(_mapper.Map<ProductResponse>(product), message: "Succeed");
                }
            }
            return new Response<ProductResponse>("Failed");
        }
    }
}
