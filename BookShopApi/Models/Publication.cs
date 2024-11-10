using BookShopApi.Interfaces;

namespace BookShopApi.Models
{
    public class Publication : IDeleteable
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public bool IsDeleted { get; set; }
        public DateTime? DeletedAt { get; set; }

        // Navigation Property
        public ICollection<Book> Books { get; set; } = new List<Book>();
    }
}
