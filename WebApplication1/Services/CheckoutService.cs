using API.Domains;
using API.DTOs.Checkouts;
using API.DTOs.Orders;
using API.DTOs.PaymentMethods;
using API.Interfaces;
using API.MoMo;
using API.VNPay;
using API.Warppers;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

namespace API.Services
{
    public class CheckoutService : ICheckoutService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContext;

        public CheckoutService(IUnitOfWork unitOfWork, IMapper mapper, IConfiguration configuration, IHttpContextAccessor httpContext)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _configuration = configuration;
            _httpContext = httpContext;
        }

        public async Task<Response<CheckOutResponse>> Checkout(CheckoutRequest request)
        {
            if (request != null)
            {
                if (request.Cart.Any())
                {
                    using (TransactionScope transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                    {
                        var status = 0;
                        var paymentMethod = await _unitOfWork.GetRepository<PaymentMethod>().GetByIdAsync(Guid.Parse(request.PaymentMethodId));
                        if (paymentMethod.Name.Equals("COD"))
                        {
                            status = 2;
                        }
                        if (paymentMethod.Name.Equals("Momo") || paymentMethod.Name.Equals("VNPay"))
                        {
                            status = -1;
                        }
                        var session = new Session
                        {
                            DateCreated = DateTime.UtcNow,
                            Id = Guid.NewGuid(),
                            PaymentMethodId = Guid.Parse(request.PaymentMethodId),
                            RetailerId = Guid.Parse(request.RetailerId),
                            ShippingAddress = request.ShippingAddress,
                            Status = status,
                            TotalCost = 0,
                        };
                        await _unitOfWork.GetRepository<Session>().AddAsync(session);
                        await _unitOfWork.SaveAsync();
                        var orders = new List<Order>();
                        double sessionCost = 0;

                        //read and add all products to orders
                        foreach (var product in request.Cart)
                        {
                            var productDetail = await _unitOfWork.GetRepository<Product>().GetByIdAsync(Guid.Parse(product.Id));
                            if (productDetail != null && productDetail.IsActive)
                            {
                                if (!orders.Exists(x => x.DistributorId.Equals(productDetail.DistributorId)))
                                {
                                    var order = new Order
                                    {
                                        DateCreated = DateTime.UtcNow,
                                        Id = Guid.NewGuid(),
                                        DistributorId = productDetail.DistributorId,
                                        OrderCost = 0,
                                        SessionId = session.Id,
                                        Status = status,
                                    };
                                    await _unitOfWork.GetRepository<Order>().AddAsync(order);
                                    await _unitOfWork.SaveAsync();
                                    orders.Add(order);
                                }
                                var prices = await _unitOfWork.GetRepository<Price>().GetAsync(x => x.ProductId.Equals(productDetail.Id), orderBy: x => x.OrderBy(y => y.Volume));
                                double orderPrice = 0;
                                foreach (var price in prices)
                                {
                                    if (product.Quantity >= price.Volume)
                                    {
                                        //discount
                                        var discountRate = 0d;
                                        var member = await _unitOfWork.GetRepository<Membership>().FirstAsync(x => x.DistributorId.Equals(productDetail.DistributorId) && x.RetailerId.Equals(Guid.Parse(request.RetailerId)));
                                        if (member != null)
                                        {
                                            var customerRank = await _unitOfWork.GetRepository<CustomerRank>().FirstAsync(x => x.DistributorId.Equals(productDetail.DistributorId) && x.MembershipRankId.Equals(member.MembershipRankId));
                                            if (customerRank != null)
                                            {
                                                discountRate = customerRank.DiscountRate;
                                            }
                                        }
                                        orderPrice = (price.Value * product.Quantity) - ((price.Value * product.Quantity) * (discountRate * 0.01));
                                    }
                                }
                                foreach (var order in orders)
                                {
                                    if (productDetail.DistributorId.Equals(order.DistributorId))
                                    {
                                        var orderDetail = new OrderDetail
                                        {
                                            DateCreated = DateTime.UtcNow,
                                            Id = Guid.NewGuid(),
                                            OrderId = order.Id,
                                            OrderPrice = orderPrice,
                                            ProductId = productDetail.Id,
                                            Quantity = product.Quantity

                                        };
                                        order.OrderCost += orderPrice;
                                        _unitOfWork.GetRepository<Order>().UpdateAsync(order);
                                        await _unitOfWork.GetRepository<OrderDetail>().AddAsync(orderDetail);
                                        await _unitOfWork.SaveAsync();
                                    }
                                }
                                productDetail.OrderTime = productDetail.OrderTime++;
                                _unitOfWork.GetRepository<Product>().UpdateAsync(productDetail);
                                await _unitOfWork.SaveAsync();
                            }
                            else
                            {
                                return new Response<CheckOutResponse>("Request is invalid");
                            }
                        }

                        //calculate totalcost
                        foreach (var order in orders)
                        {
                            sessionCost += order.OrderCost;
                        }
                        session.TotalCost = sessionCost;
                        if (sessionCost < 1000)
                        {
                            return new Response<CheckOutResponse>("Amount too low");
                        }
                        _unitOfWork.GetRepository<Session>().UpdateAsync(session);
                        await _unitOfWork.SaveAsync();

                        //paymentProcess
                        MoMoPaymentResponse paymentResponse = null;
                        string VNPayPaymentUrl = null;
                        if (status == -1)
                        {
                            if (paymentMethod.Name.Equals("Momo"))
                            {
                                var paymentRequest = new MoMoPaymentRequest
                                {
                                    Amount = (long)session.TotalCost,
                                    OrderId = session.Id.ToString(),
                                    OrderInfo = "test",
                                    RedirectUrl = request.RedirectUrl
                                };
                                var momoHandler = new MoMoHandler(_httpContext);
                                paymentResponse = momoHandler.CreatePayment(paymentRequest, _configuration);
                                if (paymentResponse.ResultCode != 0)
                                {
                                    return new Response<CheckOutResponse>("Momo payment is not connect able");
                                }
                            }
                            if (paymentMethod.Name.Equals("VNPay"))
                            {
                                var orderInfo = new VnPayOrderInfo
                                {
                                    Amount = (long)session.TotalCost,
                                    BankCode = "NCB",
                                    CreatedDate = DateTime.Now,
                                    OrderDesc = session.Id.ToString(),
                                    OrderId = DateTime.Now.Ticks,
                                    Status = status.ToString(),
                                    ReturnUrl = request.RedirectUrl,
                                };
                                VnPayMethod vnpay = new VnPayMethod(_configuration, _httpContext);
                                VNPayPaymentUrl = vnpay.CreatePayUrl(orderInfo);
                                if (VNPayPaymentUrl == null)
                                {
                                    return new Response<CheckOutResponse>("VNPay payment url cant create");
                                }
                            }

                        }
                        var response = new CheckOutResponse
                        {
                            DateCreated = session.DateCreated,
                            OrderResponses = _mapper.Map<List<OrderResponse>>(orders),
                            PaymentMethod = _mapper.Map<PaymentMethodResponse>(paymentMethod),
                            RetailerId = Guid.Parse(request.RetailerId),
                            SessionId = session.Id,
                            ShippingAddress = request.ShippingAddress,
                            TotalCost = session.TotalCost,
                            PaymentResponse = paymentResponse,
                            VNPayPaymentUrl = VNPayPaymentUrl,
                        };
                        await ChangeStatusDueToExpiredAsync(session, _unitOfWork);
                        transaction.Complete();
                        return new Response<CheckOutResponse>(response, message: "Order Succeed");
                    }
                }
            }
            return new Response<CheckOutResponse>(message: "Order Failed");
        }
        private async Task ChangeStatusDueToExpiredAsync(Session session, IUnitOfWork unitOfWork)
        {
            await Task.Delay(600);
            if (session != null)
            {
                if (session.Status == -1)
                {
                    session.Status = 0;
                    unitOfWork.GetRepository<Session>().UpdateAsync(session);
                    await unitOfWork.SaveAsync();
                    var orders = await unitOfWork.GetRepository<Order>().GetAsync(x => x.SessionId.Equals(session.Id));
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
        }
    }
}
