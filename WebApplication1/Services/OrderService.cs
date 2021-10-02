using API.Domains;
using API.DTOs.Orders;
using API.Interfaces;
using API.Warppers;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Services
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public OrderService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Response<string>> CreateOrder(CreateOrderRequest request)
        {
            var order = _mapper.Map<Order>(request);
            order.Id = Guid.NewGuid();
            order.DateCreated = DateTime.UtcNow;
            order.OrderCost = 0;
            order.Status = 1;
            await _unitOfWork.GetRepository<Order>().AddAsync(order);
            await _unitOfWork.SaveAsync();
            return new Response<string>(order.Id.ToString(), "Created");
        }

        public async Task<Response<string>> DeleteOrder(DeleteOrdetRequest request)
        {
            var order = await _unitOfWork.GetRepository<Order>().GetByIdAsync(Guid.Parse(request.Id));
            if (order != null)
            {
                var products = await _unitOfWork.GetRepository<OrderDetail>().GetAsync(x => x.OrderId.Equals(Guid.Parse(request.Id)));
                _unitOfWork.GetRepository<OrderDetail>().DeleteAllAsync(products);
                _unitOfWork.GetRepository<Order>().DeleteAsync(order);
                await _unitOfWork.SaveAsync();
                return new Response<string>(order.Id.ToString(), "Deleted");
            }
            return new Response<string>("Delete Failed");
        }

        public Task<Response<OrderResponse>> GetOrderById(GetOrderByIdRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<Response<IEnumerable<OrderResponse>>> GetOrders(GetOrdersRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<Response<string>> UpdateOrder(UpdateOrderRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
