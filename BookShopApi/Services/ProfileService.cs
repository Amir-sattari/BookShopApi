using BookShopApi.Data;
using BookShopApi.Dtos.Auth;
using BookShopApi.Interfaces;
using BookShopApi.Mappers;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace BookShopApi.Services
{
    public class ProfileService : IProfileService
    {
        private readonly IAuthService _authService;
        private readonly IShippingAddressRepository _shippingAddressRepo;
        private readonly ApplicationDbContext _context;

        public ProfileService(
            IAuthService authService,
            IShippingAddressRepository shippingAddressRepo,
            ApplicationDbContext context)
        {
            _authService = authService;
            _shippingAddressRepo = shippingAddressRepo;
            _context = context;
        }

        public async Task<ProfileOverviewDto> GetOverviewAsync(ClaimsPrincipal principal)
        {
            var user = await _authService.GetCurrentUserAsync(principal);
            var userId = user.Id;

            // DbContext is not thread-safe; keep queries sequential.
            var address = await _shippingAddressRepo.GetShippingAddressByUserIdAsync(userId);
            var orderCount = await _context.Orders.AsNoTracking().CountAsync(o => o.UserId == userId);
            var bookmarkCount = await _context.Bookmarks.AsNoTracking().CountAsync(b => b.UserId == userId);
            var stockNotifyCount = await _context.StockNotificationRequests.AsNoTracking()
                .CountAsync(r => r.UserId == userId && !r.IsNotified);
            var lastOrder = await _context.Orders.AsNoTracking()
                .Include(o => o.Items)
                .Where(o => o.UserId == userId)
                .OrderByDescending(o => o.CreatedAt)
                .FirstOrDefaultAsync();

            return new ProfileOverviewDto
            {
                User = user,
                ShippingAddress = address?.ToShippingAddressDto(),
                OrderCount = orderCount,
                BookmarkCount = bookmarkCount,
                StockNotifyCount = stockNotifyCount,
                LastOrder = lastOrder == null
                    ? null
                    : new ProfileLastOrderDto
                    {
                        Id = lastOrder.Id,
                        CreatedAt = lastOrder.CreatedAt,
                        Status = lastOrder.Status,
                        TotalAmount = lastOrder.TotalAmount,
                        ItemCount = lastOrder.Items.Sum(item => item.Quantity)
                    }
            };
        }
    }
}
