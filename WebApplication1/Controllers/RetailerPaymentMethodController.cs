using API.Contracts;
using API.DTOs.RetailerPaymentMethods;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace API.Controllers
{
    [ApiController]
    public class RetailerPaymentMethodController : ControllerBase
    {
        private readonly IRetailerPaymentMethodService _retailerPaymentMethodService;

        public RetailerPaymentMethodController(IRetailerPaymentMethodService retailerPaymentMethodService)
        {
            _retailerPaymentMethodService = retailerPaymentMethodService;
        }

        [HttpPost(ApiRoute.RetailerPaymentMethods.Create)]
        public async Task<IActionResult> Create([FromBody] CreateRetailerPaymentMethodRequest request)
        {
            var response = await _retailerPaymentMethodService.CreateRetailerPaymentMethod(request);
            if (response.Succeeded)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpGet(ApiRoute.RetailerPaymentMethods.Get)]
        public async Task<IActionResult> Get(string id)
        {
            GetRetailerPaymentMethodByIdRequest request = new GetRetailerPaymentMethodByIdRequest
            {
                Id = id
            };
            var response = await _retailerPaymentMethodService.GetRetailerPaymentMethodById(request);
            if (response.Succeeded)
            {
                return (Ok(response));
            }
            return NotFound(response);
        }

        [HttpGet(ApiRoute.RetailerPaymentMethods.GetAll)]
        public async Task<IActionResult> GetAll([FromQuery] GetRetailerPaymentMethodsRequest request)
        {
            var response = await _retailerPaymentMethodService.GetRetailerPaymentMethods(request);
            if (response.Succeeded)
            {
                return Ok(response);
            }
            return NotFound(response);
        }

        [HttpPut(ApiRoute.RetailerPaymentMethods.Update)]
        public async Task<IActionResult> Update([FromBody] UpdateRetailerPaymentMethodRequest request)
        {
            var response = await _retailerPaymentMethodService.UpdateRetailerPaymentMethod(request);
            if (response.Succeeded)
            {
                return Ok(response);
            }
            return NotFound(response);
        }
        [HttpDelete(ApiRoute.RetailerPaymentMethods.Delete)]
        public async Task<IActionResult> Delete(string id)
        {
            var request = new DeleteRetailerPaymentMethodRequest
            {
                Id = id,
            };
            var response = await _retailerPaymentMethodService.DeleteRetailerPaymentMethod(request);
            if (response.Succeeded)
            {
                return Ok(response);
            }
            return NotFound(response);
        }
    }
}
