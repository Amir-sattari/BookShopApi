using System.ComponentModel.DataAnnotations;

namespace BookShopApi.Models
{
    public class Coupon
    {
        public int Id { get; set; }

        [MaxLength(64)]
        public string Code { get; set; } = string.Empty;

        public decimal Percentage { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int? MaxUsageCount { get; set; }
        public int UsedCount { get; set; }
        public int? MaxUsagePerUser { get; set; }
        public bool IsActive { get; set; }

        public ICollection<Book> ApplicableBooks { get; set; } = new List<Book>();
        public ICollection<CouponUsage> Usages { get; set; } = new List<CouponUsage>();
    }
}
