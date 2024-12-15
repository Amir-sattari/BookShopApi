using System.ComponentModel.DataAnnotations;

namespace BookShopApi.Dtos.ShippingMethod
{
    public class CreateShippingMethodDto
    {
        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public decimal Cost { get; set; }
    }
}
