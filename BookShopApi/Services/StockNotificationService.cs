using BookShopApi.Data;
using BookShopApi.Helpers;
using BookShopApi.Interfaces;
using BookShopApi.Models;
using Microsoft.EntityFrameworkCore;

namespace BookShopApi.Services
{
    public class StockNotificationService : IStockNotificationService
    {
        private readonly ApplicationDbContext _context;
        private readonly ISmsService _smsService;
        private readonly ILogger<StockNotificationService> _logger;

        public StockNotificationService(
            ApplicationDbContext context,
            ISmsService smsService,
            ILogger<StockNotificationService> logger)
        {
            _context = context;
            _smsService = smsService;
            _logger = logger;
        }

        public async Task<StockNotificationResult> RequestNotificationAsync(int bookId, string userId)
        {
            var book = await _context.Books.FirstOrDefaultAsync(b => b.Id == bookId);
            if (book == null)
                return StockNotificationResult.BookNotFound();

            if (book.Quantity > 0)
                return StockNotificationResult.BookInStock();

            var alreadyRequested = await _context.StockNotificationRequests
                .AnyAsync(r => r.BookId == bookId && r.UserId == userId && !r.IsNotified);

            if (alreadyRequested)
                return StockNotificationResult.AlreadyRequested();

            _context.StockNotificationRequests.Add(new StockNotificationRequest
            {
                BookId = bookId,
                UserId = userId,
                RequestedAt = DateTime.UtcNow,
                IsNotified = false
            });

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                return StockNotificationResult.AlreadyRequested();
            }

            return StockNotificationResult.Created();
        }

        public async Task<bool> HasPendingRequestAsync(int bookId, string userId)
        {
            return await _context.StockNotificationRequests
                .AnyAsync(r => r.BookId == bookId && r.UserId == userId && !r.IsNotified);
        }

        public async Task<bool> CancelPendingRequestAsync(int bookId, string userId)
        {
            var request = await _context.StockNotificationRequests
                .FirstOrDefaultAsync(r => r.BookId == bookId && r.UserId == userId && !r.IsNotified);

            if (request == null)
                return false;

            _context.StockNotificationRequests.Remove(request);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task NotifySubscribersForBookAsync(int bookId)
        {
            var requests = await _context.StockNotificationRequests
                .Include(r => r.User)
                .Include(r => r.Book)
                .Where(r => r.BookId == bookId && !r.IsNotified)
                .ToListAsync();

            foreach (var request in requests)
            {
                try
                {
                    var phoneNumber = request.User?.PhoneNumber;
                    if (!string.IsNullOrWhiteSpace(phoneNumber))
                    {
                        var message = $"کتاب «{request.Book.Title}» مجدداً موجود شد.";
                        await _smsService.SendAsync(phoneNumber, message);
                    }
                    else
                    {
                        _logger.LogWarning(
                            "Skipped stock SMS for user {UserId} on book {BookId}: phone number is empty.",
                            request.UserId,
                            bookId);
                    }

                    request.IsNotified = true;
                    request.NotifiedAt = DateTime.UtcNow;
                    await _context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    _logger.LogError(
                        ex,
                        "Failed to notify user {UserId} that book {BookId} is back in stock.",
                        request.UserId,
                        bookId);
                }
            }
        }
    }
}
