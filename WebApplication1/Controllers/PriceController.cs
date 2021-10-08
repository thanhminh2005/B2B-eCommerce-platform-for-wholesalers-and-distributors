using API.Contracts;
using API.DTOs.Prices;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace API.Controllers
{
    [ApiController]
    public class PriceController : ControllerBase
    {
        private readonly IPriceService _priceService;

        public PriceController(IPriceService priceService)
        {
            _priceService = priceService;
        }

        [HttpGet(ApiRoute.Prices.GetAll)]
        public async Task<IActionResult> GetAll(string productId)
        {
            GetPricesRequest request = new GetPricesRequest
            {
                ProductId = productId
            };
            var response = await _priceService.GetPrices(request);
            if (response.Succeeded)
            {
                return Ok(response);
            }
            return NotFound(response);
        }

        [HttpGet(ApiRoute.Prices.Get)]
        public async Task<IActionResult> Get(string id)
        {
            GetPriceByIdRequest request = new GetPriceByIdRequest
            {
                Id = id
            };
            var response = await _priceService.GetPriceById(request);
            if (response.Succeeded)
            {
                return (Ok(response));
            }
            return NotFound(response);
        }

        [HttpPost(ApiRoute.Prices.Create)]
        public async Task<IActionResult> Create([FromBody] CreatePriceRequest request)
        {
            var response = await _priceService.CreatePrice(request);
            if (response.Succeeded)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpPut(ApiRoute.Prices.Update)]
        public async Task<IActionResult> Update([FromBody] UpdatePriceRequest request)
        {
            var response = await _priceService.UpdatePrice(request);
            if (response.Succeeded)
            {
                return Ok(response);
            }
            return NotFound(response);
        }

        [HttpDelete(ApiRoute.Prices.Delete)]
        public async Task<IActionResult> Delete(string id)
        {
            DeletePriceRequest request = new DeletePriceRequest
            {
                Id = id
            };
            var response = await _priceService.DeletePrice(request);
            if (response.Succeeded)
            {
                return Ok(response);
            }
            return NotFound(response);
        }
    }
}
