using API.DTOs.RetailerPaymentMethods;
using API.Warppers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Interfaces
{
    public interface IRetailerPaymentMethodService
    {
        Task<Response<string>> CreateRetailerPaymentMethod(CreateRetailerPaymentMethodRequest request);
        Task<Response<IEnumerable<RetailerPaymentMethodResponse>>> GetRetailerPaymentMethods(GetRetailerPaymentMethodsRequest request);
        Task<Response<RetailerPaymentMethodResponse>> GetRetailerPaymentMethodById(GetRetailerPaymentMethodByIdRequest request);
        Task<Response<string>> UpdateRetailerPaymentMethod(UpdateRetailerPaymentMethodRequest request);
        Task<Response<string>> DeleteRetailerPaymentMethod(DeleteRetailerPaymentMethodRequest request);

    }
}
