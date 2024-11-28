using BookShopApi.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace BookShopApi.Models
{
    public class AppUser : IdentityUser, IAuditable, IDeleteable
    {
        public string OTP { get; set; } = string.Empty;
        public DateTime? OPTExpiry { get; set; }
        public bool IsVerified { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletedAt { get; set; }

        // Navigation Property
        public ICollection<Bookmark> Bookmarks { get; set; } = new List<Bookmark>();
    }
}
