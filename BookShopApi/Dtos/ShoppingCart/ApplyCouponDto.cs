namespace BookShopApi.Dtos.ShoppingCart
{
    public class ApplyCouponDto
    {
        public string? CouponCode { get; set; }
        public List<CartItemDto>? CartItems { get; set; }
    }
}
