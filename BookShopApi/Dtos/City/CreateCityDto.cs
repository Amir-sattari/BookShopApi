using System.ComponentModel.DataAnnotations;

namespace BookShopApi.Dtos.City
{
    public class CreateCityDto
    {
        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public int ProvinceId { get; set; }
    }
}
