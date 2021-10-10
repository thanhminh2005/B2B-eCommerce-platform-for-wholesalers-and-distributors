using API.Contracts;
using API.DTOs.CustomerRanks;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace API.Controllers
{
    [ApiController]
    public class CustomerRankController : ControllerBase
    {
        private readonly ICustomerRankService _customerRankService;

        public CustomerRankController(ICustomerRankService customerRankService)
        {
            _customerRankService = customerRankService;
        }

        [HttpPost(ApiRoute.CustomerRanks.Create)]
        public async Task<IActionResult> Create([FromBody] CreateCustomerRankRequest request)
        {
            var response = await _customerRankService.CreateCustomerRank(request);
            if (response.Succeeded)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpGet(ApiRoute.CustomerRanks.Get)]
        public async Task<IActionResult> Get(string id)
        {
            GetCustomerRankByIdRequest request = new GetCustomerRankByIdRequest
            {
                Id = id
            };
            var response = await _customerRankService.GetCustomerRankById(request);
            if (response.Succeeded)
            {
                return (Ok(response));
            }
            return NotFound(response);
        }

        [HttpGet(ApiRoute.CustomerRanks.GetAll)]
        public async Task<IActionResult> GetAll([FromQuery] GetCustomerRanksRequest request)
        {
            var response = await _customerRankService.GetCustomerRanks(request);
            if (response.Succeeded)
            {
                return Ok(response);
            }
            return NotFound(response);
        }

        [HttpPut(ApiRoute.CustomerRanks.Update)]
        public async Task<IActionResult> Update([FromBody] UpdateCustomerRankRequest request)
        {
            var response = await _customerRankService.UpdateCustomerRank(request);
            if (response.Succeeded)
            {
                return Ok(response);
            }
            return NotFound(response);
        }

    }
}
