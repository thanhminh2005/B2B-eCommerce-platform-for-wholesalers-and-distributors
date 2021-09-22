using API.DTOs.Products;
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
            throw new NotImplementedException();
        }

        public async Task<Response<ProductResponse>> GetProductById(GetProductByIdRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
