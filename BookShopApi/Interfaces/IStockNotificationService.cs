using BookShopApi.Helpers;

namespace BookShopApi.Interfaces
{
    public interface IStockNotificationService
    {
        Task<StockNotificationResult> RequestNotificationAsync(int bookId, string userId);
        Task<bool> HasPendingRequestAsync(int bookId, string userId);
        Task<bool> CancelPendingRequestAsync(int bookId, string userId);
        Task NotifySubscribersForBookAsync(int bookId);
    }
}
