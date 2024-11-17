using BookShopApi.Dtos.Book;
using BookShopApi.Models;

namespace BookShopApi.Mappers
{
    public static class BookMappers
    {
        public static BookDto ToBookDto(this Book book, string imageUrl)
        {
            return new BookDto
            {
                Id = book.Id,
                Title = book.Title,
                Author = book.Author,
                Translator = book.Translator,
                Description = book.Description,
                ImageUrl = imageUrl,
                Price = book.Price,
                Quantity = book.Quantity,
                PageCount = book.PageCount,
                PrintSeries = book.PrintSeries,
                Categories = book.BookCategories.Select(bc => new Dtos.Category.CategoryDto
                {
                    Id = bc.Category.Id,
                    Name = bc.Category.Name,
                }).ToList(),
                PublicationId = book.PublicationId,
                BookSizeId = book.BookSizeId,
                CoverTypeId = book.CoverTypeId,
            };
        }

        public static Book ToBookFromCreateDto(this CreateBookDto bookDto, string imageUrl)
        {
            return new Book
            {
                Title = bookDto.Title,
                Author = bookDto.Author,
                Translator = bookDto.Translator,
                Description = bookDto.Description,
                ImageUrl = imageUrl,
                Price = bookDto.Price,
                Quantity = bookDto.Quantity,
                PageCount = bookDto.PageCount,
                PrintSeries = bookDto.PrintSeries,
                PublicationId = bookDto.PublicationId,
                BookSizeId = bookDto.BookSizeId,
                CoverTypeId = bookDto.CoverTypeId,
            };
        }

        public static void SetDataToBookFromUpdateDto(this Book book, UpdateBookDto bookDto)
        {
            book.Title = bookDto.Title;
            book.Author = bookDto.Author;
            book.Translator = bookDto.Translator;
            book.Description = bookDto.Description;
            //book.ImageUrl = imageUrl;
            book.Price = bookDto.Price;
            book.Quantity = bookDto.Quantity;
            book.PageCount = bookDto.PageCount;
            book.PrintSeries = bookDto.PrintSeries;
            book.PublicationId = bookDto.PublicationId;
            book.BookSizeId = bookDto.BookSizeId;
            book.CoverTypeId = bookDto.CoverTypeId;
        }

        public static Book SetDataToBookFromCreateDto(this CreateBookDto bookDto, string imageUrl)
        {
            return new Book
            {
                Title = bookDto.Title,
                Author = bookDto.Author,
                Translator = bookDto.Translator,
                Description = bookDto.Description,
                ImageUrl = imageUrl,
                Price = bookDto.Price,
                Quantity = bookDto.Quantity,
                PageCount = bookDto.PageCount,
                PrintSeries = bookDto.PrintSeries,
                PublicationId = bookDto.PublicationId,
                BookSizeId = bookDto.BookSizeId,
                CoverTypeId = bookDto.CoverTypeId 
            };
        }
    }
}