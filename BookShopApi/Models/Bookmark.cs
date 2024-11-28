namespace BookShopApi.Models
{
    public class Bookmark
    {
        public string UserId { get; set; } = string.Empty;
        public AppUser User { get; set; }

        public int BookId { get; set; }
        public Book Book { get; set; }
    }
}
