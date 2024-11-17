using BookShopApi.Dtos.Category;
using BookShopApi.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace BookShopApi.Dtos.Book
{
    public class UpdateBookDto : IBookDependenciesDto
    {
        [Required]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Author { get; set; } = string.Empty;

        [Required]
        public string Translator { get; set; } = string.Empty;

        [Required]
        [MinLength(128, ErrorMessage = "Description must be at least 128 characters")]
        public string Description { get; set; } = string.Empty;

        public IFormFile? ImageFile { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public int PageCount { get; set; }

        [Required]
        public int PrintSeries { get; set; }

        [Required]
        public List<int> CategoryIds { get; set; } = new List<int>();

        // Foreign Keys
        [Required]
        public int PublicationId { get; set; }

        [Required]
        public int BookSizeId { get; set; }

        [Required]
        public int CoverTypeId { get; set; }
    }
}
