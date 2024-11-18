using System.ComponentModel.DataAnnotations;

namespace BookShopApi.Dtos.Publication
{
    public class CreatePublicationDto
    {
        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public IFormFile ImageFile { get; set; }
    }
}
