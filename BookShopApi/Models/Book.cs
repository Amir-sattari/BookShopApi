using BookShopApi.Interfaces;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookShopApi.Models
{
    public class Book : IAuditable, IDeleteable
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
        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

        // Foreign Keys
        public int PublicationId { get; set; }
        public int BookSizeId { get; set; }
        public int CoverTypeId { get; set; }

        // Navigation Properties
        public Publication Publication { get; set; }
        public BookSize BookSize { get; set; }
        public CoverType CoverType { get; set; }

    }
}
