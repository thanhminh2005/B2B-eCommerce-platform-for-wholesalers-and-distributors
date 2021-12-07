using API.Domains;
using API.Interfaces;
using API.VNPay;
using API.Warppers;
using AutoMapper;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

namespace API.Services
{
    public class VNPayService : IVNPayService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public VNPayService(IUnitOfWork unitOfWork, IMapper mapper, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _configuration = configuration;
        }

        public async Task<Response<string>> SendIPNAsync(string vnpayData)
        {
            string returnString = "";
            if (vnpayData.Length > 0)
            {
                var uri = new Uri(vnpayData);
                var queryDictionary = QueryHelpers.ParseQuery(uri.Query);
                string vnp_HashSecret = _configuration.GetSection("VNPay")["HashSecret"]; ; //Chuoi bi mat
                VnPayLibrary vnpay = new VnPayLibrary();
                foreach (var s in queryDictionary)
                {
                    //get all querystring data
                    if (!string.IsNullOrEmpty(s.Key) && s.Key.StartsWith("vnp_"))
                    {
                        vnpay.AddResponseData(s.Key, s.Value);
                    }
                }
                //vnp_TxnRef: Ma don hang merchant gui VNPAY tai command=pay    
                //vnp_TransactionNo: Ma GD tai he thong VNPAY
                //vnp_ResponseCode:Response code from VNPAY: 00: Thanh cong, Khac 00: Xem tai lieu
                //vnp_SecureHash: HmacSHA512 cua du lieu tra ve

                long orderId = Convert.ToInt64(vnpay.GetResponseData("vnp_TxnRef"));
                long vnpayTranId = Convert.ToInt64(vnpay.GetResponseData("vnp_TransactionNo"));
                string vnp_ResponseCode = vnpay.GetResponseData("vnp_ResponseCode");
                string vnp_TransactionStatus = vnpay.GetResponseData("vnp_TransactionStatus");
                string vnp_SecureHash = vnpay.GetResponseData("vnp_SecureHash");
                string TerminalID = vnpay.GetResponseData("vnp_TmnCode");
                long vnp_Amount = Convert.ToInt64(vnpay.GetResponseData("vnp_Amount")) / 100;
                string bankCode = vnpay.GetResponseData("vnp_BankCode");
                string orderDesc = vnpay.GetResponseData("vnp_OrderInfo");
                bool checkSignature = vnpay.ValidateSignature(vnp_SecureHash, vnp_HashSecret);
                if (checkSignature)
                {


                    //Thanh toan thanh cong
                    using (TransactionScope transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                    {
                        var session = await _unitOfWork.GetRepository<Session>().GetByIdAsync((Guid.Parse(orderDesc)));
                        if (session != null)
                        {
                            if ((long)session.TotalCost == vnp_Amount)
                            {
                                if (session.Status == -1)
                                {
                                    if (vnp_ResponseCode == "00" && vnp_TransactionStatus == "00")
                                    {
                                        if (session.Status == -1)
                                        {
                                            session.Status = 1;
                                            _unitOfWork.GetRepository<Session>().UpdateAsync(session);
                                            await _unitOfWork.SaveAsync();
                                            var orders = await _unitOfWork.GetRepository<Order>().GetAsync(x => x.SessionId.Equals(Guid.Parse(orderDesc)));
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
                                                        var customerRank = await _unitOfWork.GetRepository<CustomerRank>().GetAsync(x => x.DistributorId.Equals(order.DistributorId));
                                                        if (customerRank.Any())
                                                        {
                                                            customerRank.OrderBy(x => x.Threshold);
                                                            foreach (var rank in customerRank)
                                                            {
                                                                if (rank.Threshold <= membership.Point)
                                                                {
                                                                    membership.MembershipRankId = rank.MembershipRankId;
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
                                                var retailer = await _unitOfWork.GetRepository<Retailer>().GetByIdAsync(session.RetailerId);
                                                var user = await _unitOfWork.GetRepository<User>().GetByIdAsync(retailer.UserId);
                                                var notification = new Notification
                                                {
                                                    DateCreate = DateTime.UtcNow,
                                                    Description = "Payment using MoMo success, Total amount: " + vnp_Amount,
                                                    Id = Guid.NewGuid(),
                                                    Title = "MomoPayment",
                                                    UserId = user.Id
                                                };
                                                await _unitOfWork.GetRepository<Notification>().AddAsync(notification);
                                                await _unitOfWork.SaveAsync();
                                            }
                                        }
                                        returnString = "Giao dịch được thực hiện thành công. Cảm ơn quý khách đã sử dụng dịch vụ";
                                        return new Response<string>(orderDesc, returnString);
                                    }
                                    else
                                    {
                                        //Thanh toan khong thanh cong. Ma loi: vnp_ResponseCode
                                        if (session != null)
                                        {
                                            if (session.Status == -1)
                                            {
                                                session.Status = 0;
                                                _unitOfWork.GetRepository<Session>().UpdateAsync(session);
                                                await _unitOfWork.SaveAsync();
                                                var orders = await _unitOfWork.GetRepository<Order>().GetAsync(x => x.SessionId.Equals(Guid.Parse(orderDesc)));
                                                if (orders.Any())
                                                {
                                                    foreach (var order in orders)
                                                    {
                                                        order.Status = 0;
                                                        _unitOfWork.GetRepository<Order>().UpdateAsync(order);
                                                        await _unitOfWork.SaveAsync();
                                                    }
                                                }
                                            }
                                        }
                                        returnString = "Có lỗi xảy ra trong quá trình xử lý.Mã lỗi: " + vnp_ResponseCode;
                                    }
                                }
                                else
                                {
                                    returnString = "Đơn hàng đã được xử lí";
                                }
                            }
                        }
                        transaction.Complete();
                    }
                }
            }
            else
            {
                returnString = "Có lỗi xảy ra trong quá trình xử lý";
            }
            return new Response<string>(returnString);
        }
    }
}
