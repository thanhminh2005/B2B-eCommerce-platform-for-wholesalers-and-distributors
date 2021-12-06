using API.Domains;
using API.Interfaces;
using API.MoMo;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

namespace API.Services
{
    public class MoMoPaymentService : IMoMoPaymentService
    {
        private readonly IConfiguration _configuration;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _httpContext;

        public MoMoPaymentService(IConfiguration configuration, IUnitOfWork unitOfWork, IHttpContextAccessor httpContext)
        {
            _configuration = configuration;
            _unitOfWork = unitOfWork;
            _httpContext = httpContext;
        }

        public async Task<IPNResponse> GetPaymentStatusAsync(IPNRequest request)
        {
            string secretkey = _configuration.GetSection("MomoKey")["SecretKey"];
            string accessKey = _configuration.GetSection("MomoKey")["AccessKey"];
            string rawHash = "accessKey=" + accessKey +
                "&amount=" + request.Amount +
                "&extraData=" + request.ExtraData +
                "&message=" + request.Message +
                "&orderId=" + request.OrderId +
                "&orderInfo=" + request.OrderInfo +
                "&orderType=" + request.OrderType +
                "&partnerCode=" + request.PartnerCode +
                "&payType=" + request.Paytype +
                "&requestId=" + request.RequestId +
                "&responseTime=" + request.ResponseTime +
                "&resultCode=" + request.ResultCode +
                "&transId=" + request.TransId
                ;
            MoMoSecurity crypto = new MoMoSecurity();
            string signature = crypto.signSHA256(rawHash, secretkey);

            string responseHash = "accessKey=" + accessKey +
                "&extraData=" + request.ExtraData +
                "&message=" + request.Message +
                "&orderId=" + request.OrderId +
                "&partnerCode=" + request.PartnerCode +
                "&requestId=" + request.RequestId +
                "&responseTime=" + request.ResponseTime +
                "&resultCode=" + request.ResultCode
                ;
            string responseSignature = crypto.signSHA256(responseHash, secretkey);
            IPNResponse response = new IPNResponse
            {
                ExtraData = request.ExtraData,
                Message = "",
                OrderId = request.OrderId,
                PartnerCode = request.PartnerCode,
                RequestId = request.OrderId,
                ResponseTime = request.ResponseTime,
                ResultCode = request.ResultCode,
                Signature = responseSignature
            };

            if (!request.Signature.Equals(signature))
            {
                response.ResultCode = 1;
                response.Message = "Signature not match";
                return response;
            }
            if (request.ResultCode == 0)
            {
                using (TransactionScope transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    var session = await _unitOfWork.GetRepository<Session>().GetByIdAsync((Guid.Parse(request.OrderId)));
                    if (session != null)
                    {
                        if (session.Status == -1)
                        {
                            session.Status = 1;
                            _unitOfWork.GetRepository<Session>().UpdateAsync(session);
                            await _unitOfWork.SaveAsync();
                            var orders = await _unitOfWork.GetRepository<Order>().GetAsync(x => x.SessionId.Equals(Guid.Parse(request.OrderId)));
                            if (orders.Any())
                            {
                                foreach (var order in orders)
                                {
                                    order.Status = 1;
                                    _unitOfWork.GetRepository<Order>().UpdateAsync(order);
                                    await _unitOfWork.SaveAsync();
                                    var membership = await _unitOfWork.GetRepository<Membership>().FirstAsync(x => x.DistributorId.Equals(order.DistributorId) && x.RetailerId.Equals(session.RetailerId));
                                    if (membership == null)
                                    {
                                        var newMembership = new Membership
                                        {
                                            DateCreated = DateTime.UtcNow,
                                            DistributorId = order.DistributorId,
                                            Point = 0,
                                            RetailerId = session.RetailerId,
                                            Id = Guid.NewGuid(),
                                        };
                                        membership = newMembership;
                                    }
                                    if (order.Status == 1)
                                    {
                                        int point = (int)Math.Floor(order.OrderCost / 1000);
                                        membership.Point += point;
                                        var customerRank = await _unitOfWork.GetRepository<CustomerRank>().GetAllAsync();
                                        if (customerRank.Any())
                                        {
                                            customerRank.OrderBy(x => x.Threshold);
                                            foreach (var rank in customerRank)
                                            {
                                                if (rank.Threshold <= membership.Point)
                                                {
                                                    membership.MembershipRankId = rank.Id;
                                                }
                                            }
                                        }
                                    }
                                    var existMembership = await _unitOfWork.GetRepository<Membership>().GetByIdAsync(membership.Id);
                                    if (existMembership == null)
                                    {
                                        await _unitOfWork.GetRepository<Membership>().AddAsync(membership);
                                    }
                                    else
                                    {
                                        existMembership.DateModified = DateTime.UtcNow;
                                        _unitOfWork.GetRepository<Membership>().UpdateAsync(membership);
                                    }
                                }
                                response.Message = "Payment succeed";
                                transaction.Complete();
                                var retailer = await _unitOfWork.GetRepository<Retailer>().GetByIdAsync(session.RetailerId);
                                var user = await _unitOfWork.GetRepository<User>().GetByIdAsync(retailer.UserId);
                                var notification = new Notification
                                {
                                    DateCreate = DateTime.UtcNow,
                                    Description = "Payment using MoMo success, Total amount: " + request.Amount,
                                    Id = Guid.NewGuid(),
                                    Title = "MomoPayment",
                                    UserId = user.Id
                                };
                                await _unitOfWork.GetRepository<Notification>().AddAsync(notification);
                                await _unitOfWork.SaveAsync();
                                //refund
                                //MoMoHandler moMoHandler = new MoMoHandler(_httpContext);

                                //var refundRequest = new RefundRequest
                                //{
                                //    Amount = request.Amount,
                                //    OrderId = request.OrderId,
                                //    PartnerCode = request.PartnerCode,
                                //    RequestId = request.RequestId,
                                //    TransId = request.TransId
                                //};
                                //var refundResponse = moMoHandler.RefundPayment(refundRequest, _configuration);
                                transaction.Complete();
                                return response;
                            }
                        }
                    }
                }
            }
            using (TransactionScope transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                var session = await _unitOfWork.GetRepository<Session>().GetByIdAsync((Guid.Parse(request.OrderId)));
                if (session != null)
                {
                    if (session.Status == -1)
                    {
                        session.Status = 0;
                        _unitOfWork.GetRepository<Session>().UpdateAsync(session);
                        await _unitOfWork.SaveAsync();
                        var orders = await _unitOfWork.GetRepository<Order>().GetAsync(x => x.SessionId.Equals(Guid.Parse(request.OrderId)));
                        if (orders.Any())
                        {
                            foreach (var order in orders)
                            {
                                order.Status = 0;
                                _unitOfWork.GetRepository<Order>().UpdateAsync(order);
                                await _unitOfWork.SaveAsync();
                            }
                            transaction.Complete();
                        }
                    }
                }
            }
            response.ResultCode = 1;
            response.Message = "There is some problem is system";
            return response;
        }
    }
}
