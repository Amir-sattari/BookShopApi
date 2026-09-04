using BookShopApi.Models;

namespace BookShopApi.Dtos.Order
{
    public class OrderDto
    {
        public int Id { get; set; }
        public string UserId { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
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
        public List<OrderItemDto> Items { get; set; } = new();
    }
}
