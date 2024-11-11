using System.ComponentModel.DataAnnotations;

namespace BookShopApi.Dtos.CoverType
{
    public class UpdateCoverTypeDto
    {
        [Required]
        public string Name { get; set; } = string.Empty;
    }
}
