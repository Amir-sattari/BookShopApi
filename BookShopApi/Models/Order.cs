using BookShopApi.Interfaces;

namespace BookShopApi.Models
{
    public class Order : IAuditable
    {
        public int Id { get; set; }
        public string UserId { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public OrderStatus Status { get; set; }

        public decimal SubTotal { get; set; }
        public decimal BookDiscountTotal { get; set; }
        public decimal CouponDiscountTotal { get; set; }
        public decimal TotalAmount { get; set; }

        public string? CouponCode { get; set; }
        public string? PaymentReferenceId { get; set; }

        public int? ShippingAddressId { get; set; }
        public string ShippingRecipientName { get; set; } = string.Empty;
        public string ShippingPhoneNumber { get; set; } = string.Empty;
        public string ShippingAddressLine { get; set; } = string.Empty;
        public string ShippingPostCode { get; set; } = string.Empty;
        public string ShippingProvinceName { get; set; } = string.Empty;
        public string ShippingCityName { get; set; } = string.Empty;

        public AppUser User { get; set; }
        public ShippingAddress? ShippingAddress { get; set; }
        public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();
    }
}
