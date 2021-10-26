using API.Contracts;
using API.Interfaces;
using API.MoMo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace API.Controllers
{
    [ApiController]
    public class MomoPaymentController : ControllerBase
    {
        private readonly IMoMoPaymentService _moMoPaymentService;

        public MomoPaymentController(IMoMoPaymentService moMoPaymentService)
        {
            _moMoPaymentService = moMoPaymentService;
        }

        [AllowAnonymous]
        [HttpPost(ApiRoute.Momo.IPN)]
        public async Task<IActionResult> IPN([FromBody] IPNRequest request)
        {
            var response = await _moMoPaymentService.GetPaymentStatusAsync(request);
            if (response.Message != "")
            {
                return Ok(response);
            }
            return BadRequest();
        }
    }
}
