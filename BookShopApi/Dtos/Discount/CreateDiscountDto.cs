using System.ComponentModel.DataAnnotations;

namespace BookShopApi.Dtos.Discount
{
    public class CreateDiscountDto
    {
        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public int Percentage { get; set; }
        
        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public int Duration { get; set; }

        [Required]
        public string DurationUnit { get; set; } = string.Empty;
    }
}
