namespace BookShopApi.Dtos.ShoppingCart
{
    public class CartPriceResult
    {
        public decimal Subtotal { get; set; }
        public decimal BookDiscountAmount { get; set; }
        public decimal CouponDiscountAmount { get; set; }
        public decimal FinalTotal { get; set; }
        public string? CouponCode { get; set; }
        public bool CouponHadNoEffect { get; set; }
        public string? Message { get; set; }
        public List<CartLinePriceResult> Items { get; set; } = new();
    }

    public class CartLinePriceResult
    {
        public int BookId { get; set; }
        public string BookTitle { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal UnitEffectivePrice { get; set; }
        public decimal LineSubtotal { get; set; }
        public decimal BookDiscountAmount { get; set; }
        public decimal CouponDiscountAmount { get; set; }
        public decimal LineTotal { get; set; }
        public string DiscountType { get; set; } = "None";
    }
}
