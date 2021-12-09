using API.Contracts;
using API.DTOs.Orders;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace API.Controllers
{
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService userService)
        {
            _orderService = userService;
        }

        [HttpPost(ApiRoute.Orders.Create)]
        public async Task<IActionResult> Create([FromBody] CreateOrderRequest request)
        {
            var response = await _orderService.CreateOrder(request);
            if (response.Succeeded)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpGet(ApiRoute.Orders.Get)]
        public async Task<IActionResult> Get(string id)
        {
            GetOrderByIdRequest request = new GetOrderByIdRequest();
            request.Id = id;
            var response = await _orderService.GetOrderById(request);
            if (response.Succeeded)
            {
                return (Ok(response));
            }
            return NotFound(response);
        }

        [HttpGet(ApiRoute.Orders.GetAll)]
        public async Task<IActionResult> GetAll([FromQuery] GetOrdersRequest request)
        {
            var response = await _orderService.GetOrders(request);
            if (response.Succeeded)
            {
                return Ok(response);
            }
            return NotFound(response);
        }

        [HttpGet(ApiRoute.Orders.GetCurrentPrice)]
        public async Task<IActionResult> GetCurrentPrice(string id)
        {
            var response = await _orderService.GetOrderCurrentPrice(id);
            if (response.Succeeded)
            {
                return Ok(response);
            }
            return NotFound(response);
        }


        [HttpPut(ApiRoute.Orders.Update)]
        public async Task<IActionResult> Update([FromBody] UpdateOrderRequest request)
        {
            var response = await _orderService.UpdateOrder(request);
            if (response.Succeeded)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpDelete(ApiRoute.Orders.Delete)]
        public async Task<IActionResult> Delete(string id)
        {
            DeleteOrderRequest request = new DeleteOrderRequest();
            request.Id = id;
            var response = await _orderService.DeleteOrder(request);
            if (response.Succeeded)
            {
                return (Ok(response));
            }
            return NotFound(response);
        }
    }
}
