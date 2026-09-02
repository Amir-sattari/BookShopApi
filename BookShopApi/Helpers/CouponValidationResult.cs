using BookShopApi.Models;

namespace BookShopApi.Helpers
{
    public class CouponValidationResult
    {
        public bool IsValid { get; }
        public string? ErrorMessage { get; }
        public Coupon? Coupon { get; }

        private CouponValidationResult(bool isValid, string? errorMessage, Coupon? coupon)
        {
            IsValid = isValid;
            ErrorMessage = errorMessage;
            Coupon = coupon;
        }

        public static CouponValidationResult Success(Coupon coupon) =>
            new(true, null, coupon);

        public static CouponValidationResult Fail(string message) =>
            new(false, message, null);
    }
}
