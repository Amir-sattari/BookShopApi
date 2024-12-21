using BookShopApi.Dtos.Discount;
using BookShopApi.Models;

namespace BookShopApi.Mappers
{
    public static class DiscountMappers
    {
        public static DiscountDto ToDiscountDto(this Discount discount)
        {
            return new DiscountDto
            {
                Id = discount.Id,
                Name = discount.Name,
                Percentage = discount.Percentage,
                Duration = discount.Duration,
                DurationUnit = discount.DurationUnit,
                StartDate = discount.StartDate,
                EndDate = discount.EndDate,
            };
        }

        public static Discount SetDataToDiscountFromCreateDto(this CreateDiscountDto discountDto, DateTime endDate)
        {
            return new Discount
            {
                Name = discountDto.Name,
                Percentage = discountDto.Percentage,
                Duration = discountDto.Duration,
                DurationUnit = discountDto.DurationUnit,
                StartDate = discountDto.StartDate,
                EndDate = endDate,
            };
        }

        public static void SetDataToDiscountFromUpdateDto(this Discount discount,  UpdateDiscountDto discountDto, DateTime endDate)
        {
            discount.Name = discountDto.Name;
            discount.Percentage = discountDto.Percentage;
            discount.Duration = discountDto.Duration;
            discount.DurationUnit = discountDto.DurationUnit;
            discount.StartDate = discountDto.StartDate;
            discount.EndDate = endDate;
        }
    }
}
