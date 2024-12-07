using Azure.Core;
using BookShopApi.Data;
using BookShopApi.Dtos.ShoppingCart;
using BookShopApi.Interfaces;
using BookShopApi.Mappers;
using BookShopApi.Models;
using Microsoft.EntityFrameworkCore;

namespace BookShopApi.Repositories
{
    public class ShoppingCartRepository : IShoppingCartRepository
    {
        private readonly ApplicationDbContext _context;

        public ShoppingCartRepository(ApplicationDbContext applicationDbContext)
        {
            _context = applicationDbContext;
        }

        public async Task AddToCartAsync(CreateShoppingCartDto cartDto)
        {
            var cartItem = await _context.ShoppingCarts
                .FirstOrDefaultAsync(item => item.UserId == cartDto.UserId && item.BookId == cartDto.BookId);

            if(cartItem != null)
            {
                cartItem.Quantity++;
            }
            else
            {
                var newCartItem = cartDto.ToShoppingCartFromCreateDto();
                await _context.ShoppingCarts.AddAsync(newCartItem);
            }

            await _context.SaveChangesAsync();
        }

        public async Task AddMultipleItemsToCartAsync(CreateMultipleShoppingCartItemsDto cartDto)
        {
            var existingItem = await _context.ShoppingCarts
                .FirstOrDefaultAsync(item => item.UserId == cartDto.UserId && item.BookId == cartDto.BookId);

            if (existingItem != null)
            {
                existingItem.Quantity += cartDto.Quantity;
            }
            else
            {
                var newCartItem = cartDto.ToShoppingCartFromCreateMultipleDto(); 
                await _context.ShoppingCarts.AddAsync(newCartItem);
            }

            await _context.SaveChangesAsync();
        }
        public async Task<ShoppingCart> IncrementCartItemQuantityAsync(UpdateShoppingCartDto cartDto)
        {
            var cartItem = await _context.ShoppingCarts.Where(s => s.UserId == cartDto.UserId && s.BookId == cartDto.BookId).Include(s => s.Book)
                .FirstOrDefaultAsync();

            if (cartItem == null)
                throw new Exception("Cart Item Not Found.");


            cartItem.Quantity++;
            await _context.SaveChangesAsync();
            return cartItem;
        }
        
        public async Task<ShoppingCart> DecrementCartItemQuantityAsync(UpdateShoppingCartDto cartDto)
        {
            var cartItem = await _context.ShoppingCarts.Where(s => s.UserId == cartDto.UserId && s.BookId == cartDto.BookId).Include(s => s.Book)
                .FirstOrDefaultAsync();

            if (cartItem == null)
                throw new Exception("Cart Item Not Found.");

            if (cartItem.Quantity == 1)
                throw new Exception("Quantity Cannot be less than 1.");

            cartItem.Quantity--;
            await _context.SaveChangesAsync();
            return cartItem;
        }
        public async Task<IEnumerable<ShoppingCart>> GetCartItemsAsync(string userId)
        {
            return await _context.ShoppingCarts.Where(s => s.UserId == userId && !s.Book.IsDeleted).Include(s => s.Book).ToListAsync();
        }

        public async Task ClearCartAsync(string userId)
        {
            var items = _context.ShoppingCarts.Where(s => s.UserId == userId);
            _context.ShoppingCarts.RemoveRange(items);
            await _context.SaveChangesAsync();
        }


        public async Task RemovFromCartAsync(string userId, int bookId)
        {
            var cartItem = await _context.ShoppingCarts
                .FirstOrDefaultAsync(s => s.UserId == userId && s.BookId == bookId);

            if (cartItem != null)
            {
                _context.ShoppingCarts.Remove(cartItem);
                await _context.SaveChangesAsync();
            }
        }

    }
}
