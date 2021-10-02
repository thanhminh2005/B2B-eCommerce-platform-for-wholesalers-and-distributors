using API.DTOs.Orders;
using API.Warppers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Interfaces
{
    public interface IOrderService
    {
        Task<Response<string>> CreateOrder(CreateOrderRequest request);
        Task<Response<IEnumerable<OrderResponse>>> GetOrders(GetOrdersRequest request);
        Task<Response<OrderResponse>> GetOrderById(GetOrderByIdRequest request);
        Task<Response<string>> UpdateOrder(UpdateOrderRequest request);
        Task<Response<string>> DeleteOrder(DeleteOrdetRequest request);
    }
}
