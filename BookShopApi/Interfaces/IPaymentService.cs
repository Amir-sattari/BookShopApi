using BookShopApi.Helpers;
using BookShopApi.Models;

namespace BookShopApi.Interfaces
{
    public interface IPaymentService
    {
        // TODO(Zarinpal): keep this contract stable so OrderService does not change when
        // Create Payment / Authority, Redirect, Callback, and Verify are implemented.
        Task<PaymentResult> ProcessPaymentAsync(Order order, CancellationToken cancellationToken = default);
    }
}
