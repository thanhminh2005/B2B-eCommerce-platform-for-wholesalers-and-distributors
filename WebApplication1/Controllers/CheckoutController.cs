using API.Contracts;
using API.DTOs.Checkouts;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace API.Controllers
{
    [ApiController]
    public class CheckoutController : ControllerBase
    {
        private readonly ICheckoutService _checkoutService;

        public CheckoutController(ICheckoutService checkoutService)
        {
            _checkoutService = checkoutService;
        }

        [HttpPost(ApiRoute.Checkouts.Confirm)]
        public async Task<IActionResult> Checkout(CheckoutRequest request)
        {
            if (request != null)
            {
                var response = await _checkoutService.Checkout(request);
                if (response.Succeeded)
                {
                    return Ok(response);
                }
                return BadRequest(response);
            }
            return BadRequest("Invalid request");
        }
    }
}
