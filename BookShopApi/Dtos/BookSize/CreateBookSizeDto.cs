using System.ComponentModel.DataAnnotations;

namespace BookShopApi.Dtos.BookSize
{
    public class CreateBookSizeDto
    {
        [Required]
        public string Name { get; set; } = string.Empty;
    }
}
