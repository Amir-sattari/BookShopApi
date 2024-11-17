using System.ComponentModel.DataAnnotations;

namespace BookShopApi.Dtos.Category
{
    public class UpdateCategoryDto
    {
        [Required]
        public string Name { get; set; } = string.Empty;

        public IFormFile? ImageFile { get; set; }
    }
}
