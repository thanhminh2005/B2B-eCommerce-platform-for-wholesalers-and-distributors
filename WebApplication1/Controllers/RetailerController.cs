using API.Contracts;
using API.DTOs.Retailers;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace API.Controllers
{
    [ApiController]
    public class RetailerController : ControllerBase
    {
        private readonly IRetailerService _retailerService;

        public RetailerController(IRetailerService retailerService)
        {
            _retailerService = retailerService;
        }

        [HttpPost(ApiRoute.Retailers.Create)]
        public async Task<IActionResult> Create([FromBody] CreateRetailerRequest request)
        {
            var response = await _retailerService.CreateRetailer(request);
            if (response.Succeeded)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpGet(ApiRoute.Retailers.Get)]
        public async Task<IActionResult> Get(string id)
        {
            GetRetailerByIdRequest request = new GetRetailerByIdRequest
            {
                Id = id
            };
            var response = await _retailerService.GetRetailerById(request);
            if (response.Succeeded)
            {
                return (Ok(response));
            }
            return NotFound(response);
        }

        [HttpGet(ApiRoute.Retailers.GetAll)]
        public async Task<IActionResult> GetAll()
        {
            var response = await _retailerService.GetRetailers();
            if (response.Succeeded)
            {
                return Ok(response);
            }
            return NotFound(response);
        }

        [HttpPut(ApiRoute.Retailers.Update)]
        public async Task<IActionResult> Update([FromBody] UpdateRetailerRequest request)
        {
            var response = await _retailerService.UpdateRetailer(request);
            if (response.Succeeded)
            {
                return Ok(response);
            }
            return NotFound(response);
        }

    }
}
