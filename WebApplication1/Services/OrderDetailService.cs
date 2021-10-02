﻿using API.Domains;
using API.DTOs.OrderDetails;
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
    public class OrderDetailService : IOrderDetailService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public OrderDetailService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Response<string>> CreateOrderDetail(CreateOrderDetailRequest request)
        {
            var product = await _unitOfWork.GetRepository<OrderDetail>().FirstAsync(x => x.ProductId.Equals(Guid.Parse(request.ProductId))
                                                                                         && x.OrderId.Equals(Guid.Parse(request.OrderId)));
            if (product == null)
            {
                var order = await _unitOfWork.GetRepository<Order>().GetByIdAsync(Guid.Parse(request.OrderId));
                if (order == null)
                {
                    var newProduct = _mapper.Map<OrderDetail>(request);
                    newProduct.DateCreated = DateTime.UtcNow;
                    newProduct.Id = Guid.NewGuid();
                    await _unitOfWork.GetRepository<OrderDetail>().AddAsync(newProduct);
                    await _unitOfWork.SaveAsync();
                    return new Response<string>(newProduct.Id.ToString(), message: "Added to order");
                }
            }
            return new Response<string>(message: "Failed to add to Order");
        }

        public async Task<Response<OrderDetailResponse>> GetOrderDetailById(GetOrderDetailByIdRequest request)
        {
            var product = await _unitOfWork.GetRepository<OrderDetail>().GetByIdAsync(Guid.Parse(request.Id));
            if (product != null)
            {
                var productInfo = _mapper.Map<ProductResponse>(await _unitOfWork.GetRepository<Product>().GetByIdAsync(product.ProductId));
                var response = _mapper.Map<OrderDetailResponse>(product);
                response.Product = productInfo;
                return new Response<OrderDetailResponse>(response, message: "Succeed");
            }
            return new Response<OrderDetailResponse>(message: "Failed");
        }

        public async Task<PagedResponse<IEnumerable<OrderDetailResponse>>> GetOrderDetails(GetOrderDetailsRequest request)
        {
            var products = await _unitOfWork.GetRepository<OrderDetail>().GetPagedReponseAsync(request.PageNumber,
                request.PageSize,
                filter: x =>
                (request.OrderId == null || x.OrderId.Equals(Guid.Parse(request.OrderId)))
                && (request.OrderPrice == null || x.OrderPrice >= request.OrderPrice)
                && (request.Quantity == null || x.Quantity >= request.Quantity),
                orderBy: x => x.OrderBy(y => y.OrderId),
                includeProperties: "Product");
            var count = await _unitOfWork.GetRepository<OrderDetail>().CountAsync(x =>
                (request.OrderId == null || x.OrderId.Equals(Guid.Parse(request.OrderId)))
                && (request.OrderPrice == null || x.OrderPrice >= request.OrderPrice)
                && (request.Quantity == null || x.Quantity >= request.Quantity));
            return new PagedResponse<IEnumerable<OrderDetailResponse>>(_mapper.Map<IEnumerable<OrderDetailResponse>>(products), request.PageNumber, request.PageSize, count);
        }

        public async Task<Response<string>> UpdateOrderDetail(UpdateOrderDetailRequest request)
        {
            var product = await _unitOfWork.GetRepository<OrderDetail>().GetByIdAsync(Guid.Parse(request.Id));
            if (product != null)
            {
                if (request.OrderPrice > 0 && request.Quantity > 0)
                {
                    product.DateModified = DateTime.UtcNow;
                    product.OrderPrice = request.OrderPrice;
                    product.Quantity = request.Quantity;
                    return new Response<string>(request.Id, message: "Order detail updated");
                }
            }
            return new Response<string>(message: "Failed to update");
        }

        public async Task<Response<string>> DeleteOrderDetail(DeleteOrderDetailRequest request)
        {
            var product = await _unitOfWork.GetRepository<OrderDetail>().GetByIdAsync(Guid.Parse(request.Id));
            if (product != null)
            {
                _unitOfWork.GetRepository<OrderDetail>().DeleteAsync(product);
                await _unitOfWork.SaveAsync();
                return new Response<string>(request.Id, message: "Order detail delete");
            }
            return new Response<string>(message: "Failed to delete");
        }
    }
}