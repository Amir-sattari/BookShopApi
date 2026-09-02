using System.ComponentModel.DataAnnotations;

namespace BookShopApi.Dtos.BookDiscount
{
    public class UpdateBookDiscountDto : IValidatableObject
    {
        [Range(0, 100)]
        public decimal Percentage { get; set; }

        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool IsActive { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (StartDate.HasValue && EndDate.HasValue && EndDate.Value <= StartDate.Value)
            {
                yield return new ValidationResult(
                    "EndDate must be after StartDate.",
                    new[] { nameof(EndDate) });
            }
        }
    }
}
