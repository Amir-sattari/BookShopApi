namespace BookShopApi.Models
{
    public class Publication
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;

        // Navigation Property
        public ICollection<Book> Books { get; set; } = new List<Book>();
    }
}
