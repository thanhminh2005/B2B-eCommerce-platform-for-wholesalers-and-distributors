using API.DTOs.Checkouts;
using API.Warppers;
using System.Threading.Tasks;

namespace API.Interfaces
{
    public interface ICheckoutService
    {
        public Task<Response<CheckOutResponse>> Checkout(CheckoutRequest request);
    }
}
