using System.ComponentModel.DataAnnotations;

namespace BookShopApi.Models
{
    public class StockNotificationRequest
    {
        public int Id { get; set; }

        public int BookId { get; set; }
        public Book Book { get; set; }

        [MaxLength(450)]
        public string UserId { get; set; } = string.Empty;
        public AppUser User { get; set; }

        public DateTime RequestedAt { get; set; }
        public bool IsNotified { get; set; }
        public DateTime? NotifiedAt { get; set; }
    }
}
