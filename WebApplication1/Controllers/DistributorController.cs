using API.Contracts;
using API.DTOs.Distributors;
using API.Interfaces;
using API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    [ApiController]
    public class DistributorController : ControllerBase
    {
        private readonly IDistributorService _distributorService;

        public DistributorController(IDistributorService distributorService)
        {
            _distributorService = distributorService;
        }

        [HttpPost(ApiRoute.Distributors.Create)]
        public async Task<IActionResult> Create([FromBody] CreateDistributorRequest request)
{
            var response = await _distributorService.CreateDistributor(request);
            if (response.Succeeded)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpGet(ApiRoute.Distributors.Get)]
        public async Task<IActionResult> Get(string id)
        {
            GetDistributorByIdRequest request = new GetDistributorByIdRequest
            {
                Id = id
            };
            var response = await _distributorService.GetDistributorById(request);
            if (response.Succeeded)
            {
                return (Ok(response));
            }
            return NotFound(response);
        }

        [HttpGet(ApiRoute.Distributors.GetAll)]
        public async Task<IActionResult> GetAll()
{
            var response = await _distributorService.GetDistributors();
            if (response.Succeeded)
            {
                return Ok(response);
            }
            return NotFound(response);
        }

        [HttpPut(ApiRoute.Distributors.Update)]
        public async Task<IActionResult> Update([FromBody] UpdateDistributorRequest request)
        {
            var response = await _distributorService.UpdateDistributor(request);
            if (response.Succeeded)
            {
                return Ok(response);
            }
            return NotFound(response);
        }

    }
}
