using BookShopApi.Constants;
using BookShopApi.Dtos.Order;
using BookShopApi.Helpers;
using BookShopApi.Interfaces;
using BookShopApi.Mappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookShopApi.Controllers.V1
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost("Checkout")]
        public async Task<IActionResult> CheckoutAsync([FromBody] CheckoutRequestDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!this.TryResolveCurrentUser(null, out var userId, out var error))
                return error!;

            var result = await _orderService.CheckoutAsync(userId, request, HttpContext.RequestAborted);

            if (result.IsSuccess && result.Order != null)
                return Created($"api/Order/{result.Order.Id}", result.Order.ToOrderDto());

            return result.Error switch
            {
                CheckoutErrorStatus.ShippingAddressNotFound =>
                    NotFound(new { Message = result.Message }),
                CheckoutErrorStatus.ShippingAddressForbidden =>
                    Forbid(),
                CheckoutErrorStatus.PaymentFailed =>
                    BadRequest(new
                    {
                        Message = result.Message,
                        Order = result.Order?.ToOrderDto()
                    }),
                _ => BadRequest(new
                {
                    Message = result.Message,
                    Order = result.Order?.ToOrderDto()
                })
            };
        }

        [HttpGet("GetAllOrders")]
        [Authorize(Roles = AppRoles.Staff)]
        public async Task<IActionResult> GetAllOrdersAsync()
        {
            var orders = await _orderService.GetAllOrdersAsync(HttpContext.RequestAborted);
            return Ok(orders.Select(o => o.ToOrderDto()));
        }

        [HttpGet("GetMyOrders")]
        public async Task<IActionResult> GetMyOrdersAsync()
        {
            if (!this.TryResolveCurrentUser(null, out var userId, out var error))
                return error!;

            var orders = await _orderService.GetOrdersByUserIdAsync(userId, HttpContext.RequestAborted);
            return Ok(orders.Select(o => o.ToOrderDto()));
        }

        [HttpGet("GetOrderById/{id:int}")]
        public async Task<IActionResult> GetOrderByIdAsync([FromRoute] int id)
        {
            if (!this.TryResolveCurrentUser(null, out var userId, out var error))
                return error!;

            var order = await _orderService.GetOrderByIdAsync(id, HttpContext.RequestAborted);
            if (order == null)
                return NotFound(new { Message = "Order not found." });

            if (order.UserId != userId && !User.IsStaff())
                return NotFound(new { Message = "Order not found." });

            return Ok(order.ToOrderDto());
        }
    }
}
