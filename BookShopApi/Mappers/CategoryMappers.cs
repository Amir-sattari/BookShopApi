using BookShopApi.Dtos.BookSize;
using BookShopApi.Dtos.Category;
using BookShopApi.Models;
using Microsoft.AspNetCore.Http;

namespace BookShopApi.Mappers
{
    public static class CategoryMappers
    {
        public static CategoryDto ToCategoryDto(this Category category, string imageUrl)
        {
            return new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                ImageUrl = imageUrl,
            };
        }

        public static Category ToCategoryFromCreateDto(this CreateCategoryDto categoryDto, string imageUrl)
        {
            return new Category
            {
                Name = categoryDto.Name,
                ImageUrl = imageUrl,
            };
        }

        public static Category SetDataToCategoryFromCreateDto(this CreateCategoryDto categoryDto, string imageUrl)
        {
            return new Category
            {
                Name = categoryDto.Name,
                ImageUrl = imageUrl,
            };
        }

        public static void SetDataToCategoryFromUpdateDto(this Category category, UpdateCategoryDto categoryDto)
        {
            category.Name = categoryDto.Name;
        }
    }
}
