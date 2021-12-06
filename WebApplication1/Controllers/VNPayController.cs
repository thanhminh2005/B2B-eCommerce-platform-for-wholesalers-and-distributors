using API.Contracts;
using API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace API.Controllers
{
    [ApiController]
    public class VNPayController : ControllerBase
    {
        private readonly IVNPayService _service;

        public VNPayController(IVNPayService service)
        {
            _service = service;
        }

        [AllowAnonymous]
        [HttpPost(ApiRoute.VNPay.IPN)]
        public async Task<IActionResult> Login(string url)
        {
            var response = await _service.SendIPNAsync(url);
            if (response.Succeeded)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
    }
}
