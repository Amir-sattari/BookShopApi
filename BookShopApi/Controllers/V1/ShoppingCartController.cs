using BookShopApi.Dtos.ShoppingCart;
using BookShopApi.Helpers;
using BookShopApi.Interfaces;
using BookShopApi.Mappers;
using BookShopApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookShopApi.Controllers.V1
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ShoppingCartController : ControllerBase
    {
        private readonly IShoppingCartRepository _shoppingCartRepo;

        public ShoppingCartController(IShoppingCartRepository shoppingCartRepository)
        {
            _shoppingCartRepo = shoppingCartRepository;
        }

        [HttpGet("GetCartItemsByUserId/{userId}")]
        public async Task<ActionResult<IEnumerable<ShoppingCart>>> GetCartItemsAsync([FromRoute] string userId)
        {
            if (!this.TryResolveCurrentUser(userId, out var currentUserId, out var error))
                return error!;

            var cartItems = await _shoppingCartRepo.GetCartItemsAsync(currentUserId);
            var cartItemsDto = cartItems.Select(c => c.ToShoppingCartDto($"{Request.Scheme}://{Request.Host}{c.Book.ImageUrl}"));
            return Ok(cartItemsDto);
        }

        [HttpPost("AddToCart")]
        public async Task<IActionResult> AddToCartAsync([FromBody] CreateShoppingCartDto cartDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!this.TryResolveCurrentUser(cartDto.UserId, out var currentUserId, out var error))
                return error!;

            cartDto.UserId = currentUserId;
            await _shoppingCartRepo.AddToCartAsync(cartDto);
            return Ok(new { Message = "Book added to cart." });
        }

        [HttpPost("AddMultipleItemsToCart")]
        public async Task<IActionResult> AddMultipleItemsToCartAsync([FromBody] CreateMultipleShoppingCartItemsDto cartDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!this.TryResolveCurrentUser(cartDto.UserId, out var currentUserId, out var error))
                return error!;

            cartDto.UserId = currentUserId;
            await _shoppingCartRepo.AddMultipleItemsToCartAsync(cartDto);
            return Ok(new { Message = "Book added to cart." });
        }

        [HttpPut("IncrementCartItemQuantity")]
        public async Task<ActionResult<ShoppingCart>> IncrementCartItemQuantityAsync([FromBody] UpdateShoppingCartDto cartDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!this.TryResolveCurrentUser(cartDto.UserId, out var currentUserId, out var error))
                return error!;

            cartDto.UserId = currentUserId;

            try
            {
                var cartItem = await _shoppingCartRepo.IncrementCartItemQuantityAsync(cartDto);
                return Ok(cartItem.ToShoppingCartDto($"{Request.Scheme}://{Request.Host}{cartItem.Book.ImageUrl}"));
            }
            catch (Exception e)
            {
                return BadRequest(new { Message = e.Message });
            }
        }
        
        [HttpPut("DecrementCartItemQuantity")]
        public async Task<ActionResult<ShoppingCart>> DecrementCartItemQuantityAsync([FromBody] UpdateShoppingCartDto cartDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!this.TryResolveCurrentUser(cartDto.UserId, out var currentUserId, out var error))
                return error!;

            cartDto.UserId = currentUserId;

            try
            {
                var cartItem = await _shoppingCartRepo.DecrementCartItemQuantityAsync(cartDto);
                return Ok(cartItem.ToShoppingCartDto($"{Request.Scheme}://{Request.Host}{cartItem.Book.ImageUrl}"));
            }
            catch (Exception e)
            {
                return BadRequest(new { Message = e.Message });
            }
        }

        [HttpDelete("RemoveFromCart/{userId}/{bookId:int}")]
        public async Task<IActionResult> RemoveFromCartAsync([FromRoute] string userId, int bookId)
        {
            if (!this.TryResolveCurrentUser(userId, out var currentUserId, out var error))
                return error!;

            await _shoppingCartRepo.RemovFromCartAsync(currentUserId, bookId);
            return NoContent();
        }

        [HttpDelete("ClearCart/{userId}")]
        public async Task<IActionResult> ClearCartAsync([FromRoute] string userId)
        {
            if (!this.TryResolveCurrentUser(userId, out var currentUserId, out var error))
                return error!;

            await _shoppingCartRepo.ClearCartAsync(currentUserId);
            return NoContent();
        }
    }
}
