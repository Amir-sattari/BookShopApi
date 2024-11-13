using BookShopApi.Dtos.BookSize;
using BookShopApi.Models;

namespace BookShopApi.Mappers
{
    public static class BookSizeMappers
    {
        public static BookSizeDto ToBookSizeDto(this BookSize bookSize)
        {
            return new BookSizeDto
            {
                Id = bookSize.Id,
                Name = bookSize.Name,
            };
        }

        public static BookSize SetDataToBookSizeFromCreateDto(this CreateBookSizeDto bookSizeDto)
        {
            return new BookSize
            {
                Name = bookSizeDto.Name,
            };
        }

        public static void SetDataToBookSizeFromUpdateDto(this BookSize bookSize, UpdateBookSizeDto bookSizeDto)
        {
            bookSize.Name = bookSizeDto.Name;
        }
    }
}
