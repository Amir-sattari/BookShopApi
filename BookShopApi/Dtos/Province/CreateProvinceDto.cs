using System.ComponentModel.DataAnnotations;

namespace BookShopApi.Dtos.Province
{
    public class CreateProvinceDto
    {
        [Required]
        public string Name { get; set; } = string.Empty;
    }
}
