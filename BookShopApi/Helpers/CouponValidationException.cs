namespace BookShopApi.Helpers
{
    public class CouponValidationException : Exception
    {
        public CouponValidationException(string message) : base(message)
        {
        }
    }
}
