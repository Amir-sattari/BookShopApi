using System.ComponentModel.DataAnnotations;

namespace BookShopApi.Dtos.Publication
{
    public class UpdatePublicationDto
    {
        [Required]
        public string Name { get; set; } = string.Empty;

        public IFormFile? ImageFile { get; set; }
    }
}
