using API.Domains;
using API.DTOs.Distributors;
using API.DTOs.Orders;
using API.Interfaces;
using API.Warppers;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<Response<string>> DeleteOrder(DeleteOrderRequest request)
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

        public async Task<Response<OrderResponse>> GetOrderById(GetOrderByIdRequest request)
        {
            var order = await _unitOfWork.GetRepository<Order>().GetByIdAsync(Guid.Parse(request.Id));
            if (order != null)
            {
                var response = _mapper.Map<OrderResponse>(order);
                var distributor = await _unitOfWork.GetRepository<Distributor>().FirstAsync(x => x.Id.Equals(order.DistributorId), includeProperties: "User");
                response.Distributor = _mapper.Map<DistributorDisplayResponse>(distributor);
                return new Response<OrderResponse>(response, message: "Succeed");
            }
            return new Response<OrderResponse>(message: "Not Found");
        }

        public async Task<Response<IEnumerable<OrderResponse>>> GetOrders(GetOrdersRequest request)
        {
            var orders = await _unitOfWork.GetRepository<Order>().GetAsync(filter: x =>
                                                                            (request.SessionId == null || x.SessionId.Equals(Guid.Parse(request.SessionId)))
                                                                            && (request.DistributorId == null || x.DistributorId.Equals(Guid.Parse(request.DistributorId)))
                                                                            && (request.Status == null || x.Status == request.Status),
                                                                            orderBy: x => x.OrderByDescending(y => y.DateCreated),
                                                                            includeProperties: "Distributor");
            if (orders.Any())
            {
                var response = _mapper.Map<IEnumerable<OrderResponse>>(orders);
                foreach (var order in response)
                {
                    var distributor = await _unitOfWork.GetRepository<Distributor>().FirstAsync(x => x.Id.Equals(order.Distributor.Id), includeProperties: "User");
                    order.Distributor = _mapper.Map<DistributorDisplayResponse>(distributor);
                }
                return new Response<IEnumerable<OrderResponse>>(response, message: "Succeed");
            }
            return new Response<IEnumerable<OrderResponse>>(message: "Empty");
        }

        public async Task<Response<string>> UpdateOrder(UpdateOrderRequest request)
        {
            var order = await _unitOfWork.GetRepository<Order>().GetByIdAsync(Guid.Parse(request.Id));
            if (order != null)
            {
                order.OrderCost = request.OrderCost;
                order.SessionId = Guid.Parse(request.SessionId);
                order.Status = request.Status;
                order.DateModified = DateTime.UtcNow;
                _unitOfWork.GetRepository<Order>().UpdateAsync(order);
                await _unitOfWork.SaveAsync();
                return new Response<string>(request.Id, message: "Updated");
            }
            return new Response<string>(message: "Update Failed");
        }

        public async Task<Response<string>> DeteleOrder(DeleteOrderRequest request)
        {
            var order = await _unitOfWork.GetRepository<Order>().GetByIdAsync(Guid.Parse(request.Id));
            if (order != null)
            {
                var products = await _unitOfWork.GetRepository<OrderDetail>().GetAsync(x => x.OrderId.Equals(Guid.Parse(request.Id)));
                if (products.Any())
                {
                    _unitOfWork.GetRepository<OrderDetail>().DeleteAllAsync(products);
                }
                _unitOfWork.GetRepository<Order>().DeleteAsync(order);
                await _unitOfWork.SaveAsync();
                return new Response<string>(request.Id, message: "Deleted");
            }
            return new Response<string>(message: "Delete Failed");
        }
    }
}
