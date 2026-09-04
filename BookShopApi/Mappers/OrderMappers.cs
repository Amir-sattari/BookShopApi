using BookShopApi.Dtos.Order;
using BookShopApi.Models;

namespace BookShopApi.Mappers
{
    public static class OrderMappers
    {
        public static OrderDto ToOrderDto(this Order order)
        {
            return new OrderDto
            {
                Id = order.Id,
                UserId = order.UserId,
                UserName = order.User?.UserName ?? string.Empty,
                CreatedAt = order.CreatedAt,
                Status = order.Status,
                SubTotal = order.SubTotal,
                BookDiscountTotal = order.BookDiscountTotal,
                CouponDiscountTotal = order.CouponDiscountTotal,
                TotalAmount = order.TotalAmount,
                CouponCode = order.CouponCode,
                PaymentReferenceId = order.PaymentReferenceId,
                ShippingAddressId = order.ShippingAddressId,
                ShippingRecipientName = order.ShippingRecipientName,
                ShippingPhoneNumber = order.ShippingPhoneNumber,
                ShippingAddressLine = order.ShippingAddressLine,
                ShippingPostCode = order.ShippingPostCode,
                ShippingProvinceName = order.ShippingProvinceName,
                ShippingCityName = order.ShippingCityName,
                Items = order.Items?.Select(i => i.ToOrderItemDto()).ToList() ?? new List<OrderItemDto>()
            };
        }

        public static OrderItemDto ToOrderItemDto(this OrderItem item)
        {
            return new OrderItemDto
            {
                Id = item.Id,
                BookId = item.BookId,
                BookTitle = item.BookTitle,
                Quantity = item.Quantity,
                UnitPrice = item.UnitPrice
            };
        }
    }
}
