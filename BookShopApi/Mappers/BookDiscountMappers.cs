using BookShopApi.Dtos.BookDiscount;
using BookShopApi.Helpers;
using BookShopApi.Models;

namespace BookShopApi.Mappers
{
    public static class BookDiscountMappers
    {
        public static BookDiscountDto ToBookDiscountDto(this BookDiscount discount)
        {
            return new BookDiscountDto
            {
                Id = discount.Id,
                BookId = discount.BookId,
                Percentage = discount.Percentage,
                StartDate = discount.StartDate,
                EndDate = discount.EndDate,
                IsActive = discount.IsActive,
                IsCurrentlyActive = discount.IsCurrentlyActive()
            };
        }

        public static BookDiscount ToBookDiscountFromCreateDto(this CreateBookDiscountDto dto, int bookId)
        {
            return new BookDiscount
            {
                BookId = bookId,
                Percentage = dto.Percentage,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                IsActive = dto.IsActive
            };
        }

        public static void SetDataToBookDiscountFromUpdateDto(this BookDiscount discount, UpdateBookDiscountDto dto)
        {
            discount.Percentage = dto.Percentage;
            discount.StartDate = dto.StartDate;
            discount.EndDate = dto.EndDate;
            discount.IsActive = dto.IsActive;
        }
    }
}
