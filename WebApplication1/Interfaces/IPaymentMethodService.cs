using API.DTOs.PaymentMethods;
using API.Warppers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Interfaces
{
    public interface IPaymentMethodService
    {
        Task<Response<string>> CreatePaymentMethod(CreatePaymentMethodRequest request);
        Task<Response<IEnumerable<PaymentMethodResponse>>> GetPaymentMethods();
        Task<Response<PaymentMethodResponse>> GetPaymentMethodById(GetPaymentMethodByIdRequest request);
        Task<Response<string>> UpdatePaymentMethod(UpdatePaymentMethodRequest request);
        Task<Response<string>> DeletePaymentMethod(DeletePaymentMethodRequest request);

    }
}
