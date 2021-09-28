using API.DTOs.OrderDetails;
using API.Warppers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Interfaces
{
    public interface IOrderDetailService
    {
        Task<Response<string>> CreateOrderDetail(CreateOrderDetailRequest request);
        Task<PagedResponse<IEnumerable<OrderDetailResponse>>> GetOrderDetails(GetOrderDetailsRequest request);
        Task<Response<OrderDetailResponse>> GetOrderDetailById(GetOrderDetailByIdRequest request);
        Task<Response<string>> UpdateOrderDetail(UpdateOrderDetailRequest request);
        Task<Response<string>> DeleteOrderDetail(DeleteOrderDetailRequest request);
    }
}
