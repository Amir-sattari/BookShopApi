using BookShopApi.Dtos.Book;
using BookShopApi.Models;

namespace BookShopApi.Mappers
{
    public static class BookMappers
    {
        public static BookDto ToBookDto(this Book book)
        {
            return new BookDto
            {
                Id = book.Id,
                Title = book.Title,
                Author = book.Author,
                Translator = book.Translator,
                Description = book.Description,
                ImageUrl = book.ImageUrl,
                Price = book.Price,
                Quantity = book.Quantity,
                PageCount = book.PageCount,
                PrintSeries = book.PrintSeries,
                PublicationId = book.PublicationId,
                BookSizeId = book.BookSizeId,
                CoverTypeId = book.CoverTypeId,
            };
        }

        public static Book ToBookFromCreateDto(this CreateBookDto bookDto)
        {
            return new Book
            {
                Title = bookDto.Title,
                Author = bookDto.Author,
                Translator = bookDto.Translator,
                Description = bookDto.Description,
                ImageUrl = bookDto.ImageUrl,
                Price = bookDto.Price,
                Quantity = bookDto.Quantity,
                PageCount = bookDto.PageCount,
                PrintSeries = bookDto.PrintSeries,
                PublicationId = bookDto.PublicationId,
                BookSizeId = bookDto.BookSizeId,
                CoverTypeId = bookDto.CoverTypeId,
            };
        }
    }
}