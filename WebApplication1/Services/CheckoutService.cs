using API.Domains;
using API.DTOs.Checkouts;
using API.DTOs.Orders;
using API.DTOs.PaymentMethods;
using API.Interfaces;
using API.MoMo;
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
                        if (paymentMethod.Name.Equals("Momo"))
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
                                        Status = status
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
                                        orderPrice = (price.Value * product.Quantity) - ((price.Value * product.Quantity) * discountRate);
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
                                if (status == 1)
                                {
                                    productDetail.OrderTime += 1;
                                    _unitOfWork.GetRepository<Product>().UpdateAsync(productDetail);
                                    await _unitOfWork.SaveAsync();
                                }
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
                        _unitOfWork.GetRepository<Session>().UpdateAsync(session);
                        await _unitOfWork.SaveAsync();

                        //paymentProcess
                        MoMoPaymentResponse paymentResponse = null;
                        if (status == -1)
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

                        var response = new CheckOutResponse
                        {
                            DateCreated = session.DateCreated,
                            OrderResponses = _mapper.Map<List<OrderResponse>>(orders),
                            PaymentMethod = _mapper.Map<PaymentMethodResponse>(paymentMethod),
                            RetailerId = Guid.Parse(request.RetailerId),
                            SessionId = session.Id,
                            ShippingAddress = request.ShippingAddress,
                            TotalCost = session.TotalCost,
                            PaymentResponse = paymentResponse
                        };
                        transaction.Complete();
                        return new Response<CheckOutResponse>(response, message: "Order Succeed");
                    }
                }
            }
            return new Response<CheckOutResponse>(message: "Order Failed");
        }
    }
}
