using System.ComponentModel.DataAnnotations;

namespace BookShopApi.Dtos.ShippingAddress
{
    public class UpdateShippingAddressDto
    {
        [Required]
        public string UserName { get; set; } = string.Empty;

        [Required]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required]
        public int ProvinceId { get; set; }

        [Required]
        public int CityId { get; set; }

        [Required]
        public string Address { get; set; } = string.Empty;

        [Required]
        public string PostCode { get; set; } = string.Empty;
    }
}
