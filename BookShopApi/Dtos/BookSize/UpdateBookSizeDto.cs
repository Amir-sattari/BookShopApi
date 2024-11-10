using System.ComponentModel.DataAnnotations;

namespace BookShopApi.Dtos.BookSize
{
    public class UpdateBookSizeDto
    {
        [Required]
        public string Name { get; set; } = string.Empty;
    }
}
