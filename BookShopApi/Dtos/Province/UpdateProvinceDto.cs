using System.ComponentModel.DataAnnotations;

namespace BookShopApi.Dtos.Province
{
    public class UpdateProvinceDto
    {
        [Required]
        public string Name { get; set; } = string.Empty;
    }
}
