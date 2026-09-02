using System.ComponentModel.DataAnnotations;

namespace BookShopApi.Dtos.Coupon
{
    public class CreateCouponDto : IValidatableObject
    {
        [Required]
        [MaxLength(64)]
        public string Code { get; set; } = string.Empty;

        [Range(0, 100)]
        public decimal Percentage { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        public int? MaxUsageCount { get; set; }
        public int? MaxUsagePerUser { get; set; }
        public bool IsActive { get; set; } = true;
        public List<int> ApplicableBookIds { get; set; } = new();

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (EndDate <= StartDate)
            {
                yield return new ValidationResult(
                    "EndDate must be after StartDate.",
                    new[] { nameof(EndDate) });
            }

            if (MaxUsageCount.HasValue && MaxUsageCount.Value < 1)
            {
                yield return new ValidationResult(
                    "MaxUsageCount must be at least 1.",
                    new[] { nameof(MaxUsageCount) });
            }

            if (MaxUsagePerUser.HasValue && MaxUsagePerUser.Value < 1)
            {
                yield return new ValidationResult(
                    "MaxUsagePerUser must be at least 1.",
                    new[] { nameof(MaxUsagePerUser) });
            }
        }
    }
}
