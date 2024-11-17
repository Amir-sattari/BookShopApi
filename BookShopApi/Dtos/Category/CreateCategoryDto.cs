using System.ComponentModel.DataAnnotations;

namespace BookShopApi.Dtos.Category
{
    public class CreateCategoryDto
    {
        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public IFormFile ImageFile { get; set; }

    }
}
