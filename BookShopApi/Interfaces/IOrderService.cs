using BookShopApi.Dtos.Order;
using BookShopApi.Helpers;
using BookShopApi.Models;

namespace BookShopApi.Interfaces
{
    public interface IOrderService
    {
        Task<CheckoutResult> CheckoutAsync(string userId, CheckoutRequestDto request, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<Order>> GetAllOrdersAsync(CancellationToken cancellationToken = default);
        Task<IReadOnlyList<Order>> GetOrdersByUserIdAsync(string userId, CancellationToken cancellationToken = default);
        Task<Order?> GetOrderByIdAsync(int id, CancellationToken cancellationToken = default);

        // Extension point for a future payment callback (Zarinpal Verify) without rewriting checkout.
        Task FinalizePaidOrderAsync(int orderId, string? paymentReferenceId, CancellationToken cancellationToken = default);
    }
}
