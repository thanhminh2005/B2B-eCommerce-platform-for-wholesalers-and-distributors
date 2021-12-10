using API.Domains;
using API.DTOs.Sessions;
using API.Interfaces;
using API.Warppers;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Services
{
    public class SessionService : ISessionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public SessionService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Response<string>> CreateSession(CreateSessionRequest request)
        {
            var session = _mapper.Map<Session>(request);
            session.Id = Guid.NewGuid();
            session.DateCreated = DateTime.UtcNow;
            session.Status = 1;
            await _unitOfWork.GetRepository<Session>().AddAsync(session);
            await _unitOfWork.SaveAsync();
            return new Response<string>(session.Id.ToString(), message: "Created");
        }

        public async Task<Response<string>> DeleteSession(DeleteSessionRequest request)
        {
            var session = await _unitOfWork.GetRepository<Session>().GetByIdAsync(Guid.Parse(request.Id));
            if (session != null)
            {
                var orders = await _unitOfWork.GetRepository<Order>().GetAsync(x => x.SessionId.Equals(Guid.Parse(request.Id)));
                if (orders.Any())
                {
                    foreach (var order in orders)
                    {
                        var products = await _unitOfWork.GetRepository<OrderDetail>().GetAsync(x => x.OrderId.Equals(order.Id));
                        _unitOfWork.GetRepository<OrderDetail>().DeleteAllAsync(products);
                    }
                    _unitOfWork.GetRepository<Order>().DeleteAllAsync(orders);

                }
                _unitOfWork.GetRepository<Session>().DeleteAsync(session);
                await _unitOfWork.SaveAsync();
                return new Response<string>(request.Id, message: "Deleted");
            }
            return new Response<string>(message: "Delete Failed");
        }

        public async Task<Response<SessionResponse>> GetSessionById(GetSessionByIdRequest request)
        {
            var session = await _unitOfWork.GetRepository<Session>().GetByIdAsync(Guid.Parse(request.Id));
            if (session != null)
            {
                var response = _mapper.Map<SessionResponse>(session);
                var retailer = await _unitOfWork.GetRepository<Retailer>().GetByIdAsync(response.RetailerId);
                var user = await _unitOfWork.GetRepository<User>().GetByIdAsync(retailer.UserId);
                response.RetailerName = user.DisplayName;
                return new Response<SessionResponse>(response, message: "Succeed");
            }
            return new Response<SessionResponse>(message: "Not Found");
        }
        public async Task<PagedResponse<IEnumerable<SessionResponse>>> GetSessions(GetSessionsRequest request)
        {
            var sessions = await _unitOfWork.GetRepository<Session>().GetPagedReponseAsync(request.PageNumber, request.PageSize, filter: x =>
            (request.PaymentMethodId == null || x.PaymentMethodId.Equals(Guid.Parse(request.PaymentMethodId)))
            && (request.RetailerId == null || x.RetailerId.Equals(Guid.Parse(request.RetailerId))),
            orderBy: x => x.OrderByDescending(y => y.DateCreated),
            includeProperties: "PaymentMethod"
            );

            var count = await _unitOfWork.GetRepository<Session>().CountAsync(x =>
            (request.PaymentMethodId == null || x.PaymentMethodId.Equals(Guid.Parse(request.PaymentMethodId)))
            && (request.RetailerId == null || x.RetailerId.Equals(Guid.Parse(request.RetailerId))));
            var response = _mapper.Map<IEnumerable<SessionResponse>>(sessions);
            foreach (var session in response)
            {
                var retailer = await _unitOfWork.GetRepository<Retailer>().GetByIdAsync(session.RetailerId);
                var user = await _unitOfWork.GetRepository<User>().GetByIdAsync(retailer.UserId);
                session.RetailerName = user.DisplayName;
            }

            return new PagedResponse<IEnumerable<SessionResponse>>(response, request.PageNumber, request.PageSize, count);
        }

        //public async Task<Response<IEnumerable<SessionResponse>>> GetSessions(GetSessionsRequest request)
        //{
        //    var sessions = await _unitOfWork.GetRepository<Session>().GetAsync(filter: x =>
        //    (request.PaymentMethodId == null || x.PaymentMethodId.Equals(Guid.Parse(request.PaymentMethodId)))
        //    && (request.RetailerId == null || x.RetailerId.Equals(Guid.Parse(request.RetailerId))),
        //    orderBy: x => x.OrderByDescending(y => y.DateCreated), 
        //    includeProperties: "PaymentMethod"
        //    );
        //    if (sessions.Any())
        //    {
        //        return new Response<IEnumerable<SessionResponse>>(_mapper.Map<IEnumerable<SessionResponse>>(sessions), message: "Succeed");
        //    }
        //    return new Response<IEnumerable<SessionResponse>>(message: "Empty");
        //}

        public async Task<Response<string>> UpdateSession(UpdateSessionRequest request)
        {
            var session = await _unitOfWork.GetRepository<Session>().GetByIdAsync(Guid.Parse(request.Id));
            if (session != null)
            {
                session.DateModified = DateTime.UtcNow;
                session.PaymentMethodId = Guid.Parse(request.PaymentMethodId);
                session.ShippingAddress = request.ShippingAddress;
                session.Status = request.Status;
                session.TotalCost = request.TotalCost;
                _unitOfWork.GetRepository<Session>().UpdateAsync(session);
                await _unitOfWork.SaveAsync();
                return new Response<string>(request.Id, message: "Updated");
            }
            return new Response<string>(message: "Update Failed");
        }
    }
}
