using API.DTOs.Orders;
using API.Warppers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Interfaces
{
    public interface IOrderService
    {
        Task<Response<string>> CreateOrder(CreateOrderRequest request);
        Task<PagedResponse<IEnumerable<OrderResponse>>> GetOrders(GetOrdersRequest request);
        Task<Response<OrderResponse>> GetOrderById(GetOrderByIdRequest request);
        Task<Response<double>> GetOrderCurrentPrice(string id);
        Task<Response<string>> UpdateOrder(UpdateOrderRequest request);
        Task<Response<string>> DeleteOrder(DeleteOrderRequest request);
    }
}
