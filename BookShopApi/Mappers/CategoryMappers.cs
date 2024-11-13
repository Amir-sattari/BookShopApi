using BookShopApi.Dtos.BookSize;
using BookShopApi.Dtos.Category;
using BookShopApi.Models;

namespace BookShopApi.Mappers
{
    public static class CategoryMappers
    {
        public static CategoryDto ToCategoryDto(this Category category)
        {
            return new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
            };
        }

        public static Category ToCategoryFromCreateDto(this CreateCategoryDto categoryDto)
        {
            return new Category
            {
                Name = categoryDto.Name
            };
        }

        public static Category SetDataToCategoryFromCreateDto(this CreateCategoryDto categoryDto)
        {
            return new Category
            {
                Name = categoryDto.Name,
            };
        }

        public static void SetDataToCategoryFromUpdateDto(this Category category, UpdateCategoryDto categoryDto)
        {
            category.Name = categoryDto.Name;
        }
    }
}
