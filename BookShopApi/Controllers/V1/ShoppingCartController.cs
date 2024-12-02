using BookShopApi.Dtos.ShoppingCart;
using BookShopApi.Interfaces;
using BookShopApi.Mappers;
using BookShopApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace BookShopApi.Controllers.V1
{
    [Route("api/[controller]")]
    [ApiController]
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
            var cartItems = await _shoppingCartRepo.GetCartItemsAsync(userId);
            var cartItemsDto = cartItems.Select(c => c.ToShoppingCartDto($"{Request.Scheme}://{Request.Host}{c.Book.ImageUrl}"));
            return Ok(cartItemsDto);
        }

        [HttpPost("AddToCart")]
        public async Task<IActionResult> AddToCartAsync([FromBody] CreateShoppingCartDto cartDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _shoppingCartRepo.AddToCartAsync(cartDto);
            return Ok(new { Message = "Book added to cart." });
        }

        [HttpPut("IncrementCartItemQuantity")]
        public async Task<ActionResult<ShoppingCart>> IncrementCartItemQuantityAsync([FromBody] UpdateShoppingCartDto cartDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

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
            if (!ModelState.IsValid)
                return BadRequest(ModelState);


            await _shoppingCartRepo.RemovFromCartAsync(userId, bookId);
            return NoContent();
        }

        [HttpDelete("ClearCart/{userId}")]
        public async Task<IActionResult> ClearCartAsync([FromRoute] string userId)
        {
            await _shoppingCartRepo.ClearCartAsync(userId);
            return NoContent();
        }
    }
}
