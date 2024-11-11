using System.ComponentModel.DataAnnotations;

namespace BookShopApi.Dtos.CoverType
{
    public class CreateCoverTypeDto
    {
        [Required]
        public string Name { get; set; } = string.Empty;
    }
}
