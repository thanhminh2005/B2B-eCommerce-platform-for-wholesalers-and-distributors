using API.Domains;
using API.DTOs.Distributors;
using API.DTOs.Orders;
using API.DTOs.Retailers;
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
                var sessions = await _unitOfWork.GetRepository<Session>().GetAsync(x => x.Id.Equals(order.SessionId), includeProperties: "PaymentMethod,Retailer");
                var session = sessions.ElementAt(0);
                var user = await _unitOfWork.GetRepository<User>().GetByIdAsync(session.Retailer.UserId);
                session.Retailer.User = user;
                response.Retailer = _mapper.Map<RetailerDisplayResponse>(session.Retailer);
                response.Distributor = _mapper.Map<DistributorDisplayResponse>(distributor);
                return new Response<OrderResponse>(response, message: "Succeed");
            }
            return new Response<OrderResponse>(message: "Not Found");
        }

        public async Task<PagedResponse<IEnumerable<OrderResponse>>> GetOrders(GetOrdersRequest request)
        {
            IEnumerable<Order> orders = null;
            var count = 0;
            List<int> statusList = new List<int>();
            if (!string.IsNullOrWhiteSpace(request.Status))
            {
                foreach (var item in request.Status.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    statusList.Add(int.Parse(item));
                }
            }
            if (string.IsNullOrWhiteSpace(request.SessionId) && !string.IsNullOrWhiteSpace(request.RetailerId))
            {
                var session = await _unitOfWork.GetRepository<Session>().GetAsync(x => x.RetailerId.Equals(Guid.Parse(request.RetailerId)),
                    orderBy: x => x.OrderBy(y => y.DateCreated));
                orders = await _unitOfWork.GetRepository<Order>().GetPagedReponseAsync(request.PageNumber,
                    request.PageSize,
                    x => (request.Status == null || statusList.Contains(x.Status))
                    && (session.Select(y => y.Id).Contains(x.SessionId))
                    && (request.DistributorId == null || x.DistributorId == Guid.Parse(request.DistributorId)),
                    orderBy: x => x.OrderBy(y => y.DateCreated),
                    includeProperties: "Distributor");
                count = await _unitOfWork.GetRepository<Order>().CountAsync(
                    x => (request.Status == null || statusList.Contains(x.Status))
                    && (session.Select(y => y.Id).Contains(x.SessionId))
                    && (request.DistributorId == null || x.DistributorId == Guid.Parse(request.DistributorId)));
            }
            else
            {
                orders = await _unitOfWork.GetRepository<Order>().GetPagedReponseAsync(request.PageNumber,
                                                                                        request.PageSize,
                                                                                        filter: x =>
                                                                                        (request.SessionId == null || x.SessionId.Equals(Guid.Parse(request.SessionId)))
                                                                                        && (request.DistributorId == null || x.DistributorId.Equals(Guid.Parse(request.DistributorId)))
                                                                                        && (request.Status == null || statusList.Contains(x.Status)),
                                                                                        orderBy: x => x.OrderBy(y => y.DateCreated),
                                                                                        includeProperties: "Distributor");
                count = await _unitOfWork.GetRepository<Order>().CountAsync(filter: x =>
                                                                                (request.SessionId == null || x.SessionId.Equals(Guid.Parse(request.SessionId)))
                                                                                && (request.DistributorId == null || x.DistributorId.Equals(Guid.Parse(request.DistributorId)))
                                                                                && (request.Status == null || statusList.Contains(x.Status)));
            }
            var response = _mapper.Map<IEnumerable<OrderResponse>>(orders);
            foreach (var order in response)
            {
                var sessions = await _unitOfWork.GetRepository<Session>().GetAsync(x => x.Id.Equals(order.SessionId), includeProperties: "PaymentMethod,Retailer");
                var session = sessions.ElementAt(0);
                var user = await _unitOfWork.GetRepository<User>().GetByIdAsync(session.Retailer.UserId);
                session.Retailer.User = user;
                order.Retailer = _mapper.Map<RetailerDisplayResponse>(session.Retailer);
                var distributor = await _unitOfWork.GetRepository<Distributor>().FirstAsync(x => x.Id.Equals(order.Distributor.Id), includeProperties: "User");
                order.Distributor = _mapper.Map<DistributorDisplayResponse>(distributor);
            }
            return new PagedResponse<IEnumerable<OrderResponse>>(response, request.PageNumber, request.PageSize, count);
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
