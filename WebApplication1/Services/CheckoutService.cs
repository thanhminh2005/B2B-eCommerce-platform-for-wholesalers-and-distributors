using API.Domains;
using API.DTOs.Checkouts;
using API.Interfaces;
using API.Warppers;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Services
{
    public class CheckoutService : ICheckoutService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CheckoutService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Response<string>> Checkout(CheckoutRequest request)
        {
            if (request != null)
            {
                if (request.Cart.Count() != 0)
                {
                    var status = 1;
                    var paymentMethod = await _unitOfWork.GetRepository<PaymentMethod>().FirstAsync(x => x.Id.Equals(Guid.Parse(request.PaymentMethodId)));
                    if (paymentMethod.Name.Equals("COD"))
                    {
                        status = 2;
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

                    var orders = new List<Order>();
                    double sessionCost = 0;
                    foreach (var product in request.Cart)
                    {
                        var productDetail = await _unitOfWork.GetRepository<Product>().GetByIdAsync(Guid.Parse(product.Id));
                        if (productDetail != null)
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
                                orders.Add(order);
                            }
                            var prices = await _unitOfWork.GetRepository<Price>().GetAsync(x => x.ProductId.Equals(productDetail.Id));
                            double orderPrice = 0;
                            foreach (var price in prices)
                            {
                                if (product.Quantity <= price.Volume)
                                {
                                    orderPrice = price.Value * product.Quantity;
                                }
                            }
                            foreach (var order in orders)
                            {
                                if (productDetail.DistributorId.Equals(order))
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
                                    await _unitOfWork.GetRepository<OrderDetail>().AddAsync(orderDetail);
                                    order.OrderCost += orderPrice;
                                    sessionCost += orderPrice;
                                    _unitOfWork.GetRepository<Order>().UpdateAsync(order);
                                }
                            }
                        }
                    }
                    var memberships = new List<Membership>();
                    foreach (var order in orders)
                    {
                        var membership = await _unitOfWork.GetRepository<Membership>().FirstAsync(x => x.DistributorId.Equals(order.DistributorId) && x.RetailerId.Equals(Guid.Parse(request.RetailerId)));
                        if (membership == null)
                        {
                            var ranks = await _unitOfWork.GetRepository<CustomerRank>()
                                                          .GetAsync(x => x.DistributorId.Equals(order.DistributorId),
                                                                          x => x.OrderByDescending(y => y.Threshold));
                            var rank = ranks.FirstOrDefault(x => x.Threshold < membership.Point);
                            if (rank != null)
                            {

                            }
                            var newMembership = new Membership
                            {
                                DateCreated = DateTime.UtcNow,
                                DistributorId = order.DistributorId,
                                Point = 0,
                            };
                        }
                    }
                    session.TotalCost = sessionCost;
                    _unitOfWork.GetRepository<Session>().UpdateAsync(session);
                    await _unitOfWork.SaveAsync();
                    return new Response<string>(session.Id.ToString(), message: "Order Succeed");
                }
            }
            return new Response<string>(message: "Order Failed");
        }
    }
}
