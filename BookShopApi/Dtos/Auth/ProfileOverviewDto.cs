using BookShopApi.Dtos.ShippingAddress;
using BookShopApi.Models;

namespace BookShopApi.Dtos.Auth
{
    public class ProfileOverviewDto
    {
        public CurrentUserDto User { get; set; } = new();
        public ShippingAddressDto? ShippingAddress { get; set; }
        public int OrderCount { get; set; }
        public int BookmarkCount { get; set; }
        public int StockNotifyCount { get; set; }
        public ProfileLastOrderDto? LastOrder { get; set; }
    }

    public class ProfileLastOrderDto
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public OrderStatus Status { get; set; }
        public decimal TotalAmount { get; set; }
        public int ItemCount { get; set; }
    }
}
