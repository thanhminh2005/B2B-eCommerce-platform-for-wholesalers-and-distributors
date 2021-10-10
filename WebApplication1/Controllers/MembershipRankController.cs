using API.Contracts;
using API.DTOs.MembershipRanks;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace API.Controllers
{
    [ApiController]
    public class MembershipRankController : ControllerBase
    {
        private readonly IMembershipRankService _membershipRankService;

        public MembershipRankController(IMembershipRankService membershipRankService)
        {
            _membershipRankService = membershipRankService;
        }

        [HttpPost(ApiRoute.MembershipRanks.Create)]
        public async Task<IActionResult> Create([FromBody] CreateMembershipRankRequest request)
        {
            var response = await _membershipRankService.CreateMembershipRank(request);
            if (response.Succeeded)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpGet(ApiRoute.MembershipRanks.Get)]
        public async Task<IActionResult> Get(string id)
        {
            GetMembershipRankByIdRequest request = new GetMembershipRankByIdRequest
            {
                Id = id
            };
            var response = await _membershipRankService.GetMembershipRankById(request);
            if (response.Succeeded)
            {
                return (Ok(response));
            }
            return NotFound(response);
        }

        [HttpGet(ApiRoute.MembershipRanks.GetAll)]
        public async Task<IActionResult> GetAll()
        {
            var response = await _membershipRankService.GetMembershipRanks();
            if (response.Succeeded)
            {
                return Ok(response);
            }
            return NotFound(response);
        }

        [HttpPut(ApiRoute.MembershipRanks.Update)]
        public async Task<IActionResult> Update([FromBody] UpdateMembershipRankRequest request)
        {
            var response = await _membershipRankService.UpdateMembershipRank(request);
            if (response.Succeeded)
            {
                return Ok(response);
            }
            return NotFound(response);
        }

    }
}
