using System.ComponentModel.DataAnnotations;

namespace BookShopApi.Dtos.Order
{
    public class CheckoutRequestDto
    {
        [Required]
        public int ShippingAddressId { get; set; }

        public string? CouponCode { get; set; }
    }
}
