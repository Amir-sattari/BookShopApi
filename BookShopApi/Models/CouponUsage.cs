namespace BookShopApi.Models
{
    public class CouponUsage
    {
        public int Id { get; set; }
        public int CouponId { get; set; }
        public Coupon Coupon { get; set; }
        public string UserId { get; set; } = string.Empty;
        public DateTime UsedAt { get; set; }
    }
}
