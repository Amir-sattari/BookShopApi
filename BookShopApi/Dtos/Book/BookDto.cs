using BookShopApi.Models;

namespace BookShopApi.Dtos.Book
{
    public class BookDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public string Translator { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public int PageCount { get; set; }
        public int PrintSeries { get; set; }

        // Foreign Keys
        public int PublicationId { get; set; }
        public int BookSizeId { get; set; }
        public int CoverTypeId { get; set; }

    }
}
