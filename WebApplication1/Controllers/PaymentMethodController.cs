using API.Contracts;
using API.DTOs.PaymentMethods;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace API.Controllers
{
    [ApiController]
    public class PaymentMethodController : ControllerBase
    {
        private readonly IPaymentMethodService _paymentMethodService;

        public PaymentMethodController(IPaymentMethodService paymentMethodService)
        {
            _paymentMethodService = paymentMethodService;
        }

        [HttpPost(ApiRoute.PaymentMethods.Create)]
        public async Task<IActionResult> Create([FromBody] CreatePaymentMethodRequest request)
        {
            var response = await _paymentMethodService.CreatePaymentMethod(request);
            if (response.Succeeded)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpGet(ApiRoute.PaymentMethods.Get)]
        public async Task<IActionResult> Get(string id)
        {
            GetPaymentMethodByIdRequest request = new GetPaymentMethodByIdRequest
            {
                Id = id
            };
            var response = await _paymentMethodService.GetPaymentMethodById(request);
            if (response.Succeeded)
            {
                return (Ok(response));
            }
            return NotFound(response);
        }

        [HttpGet(ApiRoute.PaymentMethods.GetAll)]
        public async Task<IActionResult> GetAll()
        {
            var response = await _paymentMethodService.GetPaymentMethods();
            if (response.Succeeded)
            {
                return Ok(response);
            }
            return NotFound(response);
        }

        [HttpPut(ApiRoute.PaymentMethods.Update)]
        public async Task<IActionResult> Update([FromBody] UpdatePaymentMethodRequest request)
        {
            var response = await _paymentMethodService.UpdatePaymentMethod(request);
            if (response.Succeeded)
            {
                return Ok(response);
            }
            return NotFound(response);
        }

        [HttpDelete(ApiRoute.PaymentMethods.Delete)]
        public async Task<IActionResult> Delete(string id)
        {
            DeletePaymentMethodRequest request = new DeletePaymentMethodRequest
            {
                Id = id
            };
            var response = await _paymentMethodService.DeletePaymentMethod(request);
            if (response.Succeeded)
            {
                return Ok(response);
            }
            return NotFound(response);
        }

    }
}
