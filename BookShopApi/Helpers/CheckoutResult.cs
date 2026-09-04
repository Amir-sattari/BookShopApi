using BookShopApi.Models;

namespace BookShopApi.Helpers
{
    public enum CheckoutErrorStatus
    {
        EmptyCart,
        ShippingAddressNotFound,
        ShippingAddressForbidden,
        InsufficientStock,
        InvalidCoupon,
        PaymentFailed,
        FinalizationFailed
    }

    public class CheckoutResult
    {
        public bool IsSuccess { get; init; }
        public CheckoutErrorStatus? Error { get; init; }
        public string? Message { get; init; }
        public Order? Order { get; init; }

        public static CheckoutResult Succeeded(Order order) => new()
        {
            IsSuccess = true,
            Order = order
        };

        public static CheckoutResult Failed(CheckoutErrorStatus error, string message, Order? order = null) => new()
        {
            IsSuccess = false,
            Error = error,
            Message = message,
            Order = order
        };
    }
}
