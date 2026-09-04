using BookShopApi.Helpers;
using BookShopApi.Interfaces;
using BookShopApi.Models;

namespace BookShopApi.Services
{
    public class StubPaymentService : IPaymentService
    {
        public Task<PaymentResult> ProcessPaymentAsync(Order order, CancellationToken cancellationToken = default)
        {
            // Default stub: treat payment as successful so checkout can be tested end-to-end.
            // Swap this registration for a Zarinpal implementation later; OrderService stays the same.
            //
            // TODO(Zarinpal):
            // 1. Create Payment / Authority — request an authority for order.TotalAmount
            // 2. Redirect — send the user to the Zarinpal payment page
            // 3. Callback — accept the gateway return and load the pending order
            // 4. Verify — confirm the payment with Zarinpal, then call IOrderService.FinalizePaidOrderAsync
            cancellationToken.ThrowIfCancellationRequested();
            var referenceId = $"STUB-{order.Id}-{Guid.NewGuid():N}";
            return Task.FromResult(PaymentResult.Success(referenceId));
        }
    }
}
