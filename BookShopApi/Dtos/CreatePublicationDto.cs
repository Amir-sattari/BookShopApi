using System.ComponentModel.DataAnnotations;

namespace BookShopApi.Dtos
{
    public class CreatePublicationDto
    {
        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string ImageUrl { get; set; } = string.Empty;
    }
}
