using BookShopApi.Dtos.BookSize;
using BookShopApi.Dtos.Category;
using BookShopApi.Models;
using Microsoft.AspNetCore.Http;

namespace BookShopApi.Mappers
{
    public static class CategoryMappers
    {
        public static CategoryDto ToCategoryDto(this Category category, string imageUrl = "")
        {
            return new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                ImageUrl = imageUrl,
            };
        }

        //public static Category ToCategoryFromCreateDto(this CreateCategoryDto categoryDto)
        //{
        //    return new Category
        //    {
        //        Name = categoryDto.Name,
        //        ImageUrl = categoryDto.ImageUrl,
        //    };
        //}

        //public static Category SetDataToCategoryFromCreateDto(this CreateCategoryDto categoryDto)
        //{
        //    return new Category
        //    {
        //        Name = categoryDto.Name,
        //        ImageUrl = categoryDto.ImageUrl,
        //    };
        //}

        //public static void SetDataToCategoryFromUpdateDto(this Category category, UpdateCategoryDto categoryDto)
        //{
        //    category.Name = categoryDto.Name;
        //    category.ImageUrl = categoryDto.ImageUrl;
        //}
    }
}
