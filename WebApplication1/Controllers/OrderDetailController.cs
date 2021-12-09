using API.Contracts;
using API.DTOs.OrderDetails;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace API.Controllers
{
    [ApiController]
    public class OrderDetailController : ControllerBase
    {
        private readonly IOrderDetailService _orderDetailService;

        public OrderDetailController(IOrderDetailService userService)
        {
            _orderDetailService = userService;
        }

        [HttpPost(ApiRoute.OrderDetails.Create)]
        public async Task<IActionResult> Create([FromBody] CreateOrderDetailRequest request)
        {
            var response = await _orderDetailService.CreateOrderDetail(request);
            if (response.Succeeded)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpGet(ApiRoute.OrderDetails.Get)]
        public async Task<IActionResult> Get(string id)
        {
            GetOrderDetailByIdRequest request = new GetOrderDetailByIdRequest
            {
                Id = id
            };
            var response = await _orderDetailService.GetOrderDetailById(request);
            if (response.Succeeded)
            {
                return (Ok(response));
            }
            return NotFound(response);
        }

        [HttpGet(ApiRoute.OrderDetails.GetAll)]
        public async Task<IActionResult> GetAll([FromQuery] GetOrderDetailsRequest request)
        {
            var response = await _orderDetailService.GetOrderDetails(request);
            if (response.Succeeded)
            {
                return Ok(response);
            }
            return NotFound(response);
        }


        [HttpPut(ApiRoute.OrderDetails.Update)]
        public async Task<IActionResult> Update([FromBody] UpdateOrderDetailRequest request)
        {
            var response = await _orderDetailService.UpdateOrderDetail(request);
            if (response.Succeeded)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpDelete(ApiRoute.OrderDetails.Delete)]
        public async Task<IActionResult> Delete(string id)
        {
            DeleteOrderDetailRequest request = new DeleteOrderDetailRequest
            {
                Id = id
            };
            var response = await _orderDetailService.DeleteOrderDetail(request);
            if (response.Succeeded)
            {
                return (Ok(response));
            }
            return NotFound(response);
        }
    }
}
