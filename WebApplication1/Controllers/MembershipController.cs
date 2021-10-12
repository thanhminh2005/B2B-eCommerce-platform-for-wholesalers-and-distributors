using API.Contracts;
using API.DTOs.Memberships;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace API.Controllers
{
    [ApiController]
    public class MembershipController : ControllerBase
    {
        private readonly IMembershipService _membershipService;

        public MembershipController(IMembershipService membershipService)
        {
            _membershipService = membershipService;
        }

        [HttpPost(ApiRoute.Memberships.Create)]
        public async Task<IActionResult> Create([FromBody] CreateMembershipRequest request)
        {
            var response = await _membershipService.CreateMembership(request);
            if (response.Succeeded)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpGet(ApiRoute.Memberships.Get)]
        public async Task<IActionResult> Get([FromRoute(Name = "distributor-id")] string distributorId, [FromRoute(Name = "retailer-id")] string retailerId)
        {
            GetMembershipByIdRequest request = new GetMembershipByIdRequest
            {
                DistributorId = distributorId,
                RetailerId = retailerId
            };
            var response = await _membershipService.GetMembershipById(request);
            if (response.Succeeded)
            {
                return (Ok(response));
            }
            return NotFound(response);
        }

        [HttpGet(ApiRoute.Memberships.GetAll)]
        public async Task<IActionResult> GetAll([FromQuery] GetMembershipsRequest request)
        {
            var response = await _membershipService.GetMemberships(request);
            if (response.Succeeded)
            {
                return Ok(response);
            }
            return NotFound(response);
        }

        [HttpPut(ApiRoute.Memberships.Update)]
        public async Task<IActionResult> Update([FromBody] UpdateMembershipRequest request)
        {
            var response = await _membershipService.UpdateMembership(request);
            if (response.Succeeded)
            {
                return Ok(response);
            }
            return NotFound(response);
        }

    }
}
