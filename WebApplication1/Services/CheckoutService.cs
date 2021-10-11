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
                    var paymentMethod = await _unitOfWork.GetRepository<PaymentMethod>().FirstAsync(x => x.Id.Equals(Guid.Parse(request.PaymentMethodId)));
                    var status = 1;
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
                    
                    var orders = new List<Guid>();
                    double sessionCost = 0;
                    foreach (var product in request.Cart)
                    {
                        var productDetail = await _unitOfWork.GetRepository<Product>().GetByIdAsync(Guid.Parse(product.Id));
                        if (productDetail != null)
                        {
                            if (!orders.Contains(productDetail.DistributorId))
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
                                orders.Add(order.Id);
                                await _unitOfWork.GetRepository<Order>().AddAsync(order);
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
                            foreach (var orderid in orders)
                            {
                                if (productDetail.DistributorId.Equals(orderid))
                                {
                                    var orderDetail = new OrderDetail
                                    {
                                        DateCreated = DateTime.UtcNow,
                                        Id = Guid.NewGuid(),
                                        OrderId = orderid,
                                        OrderPrice = orderPrice,
                                        ProductId = productDetail.Id,
                                        Quantity = product.Quantity

                                    };
                                    await _unitOfWork.GetRepository<OrderDetail>().AddAsync(orderDetail);
                                    var orderPriceUpdateObj = await _unitOfWork.GetRepository<Order>().GetByIdAsync(orderid);
                                    orderPriceUpdateObj.OrderCost += orderPrice;
                                    
                                    sessionCost += orderPrice;
                                    _unitOfWork.GetRepository<Order>().UpdateAsync(orderPriceUpdateObj);
                                }
                            }
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
