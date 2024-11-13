using BookShopApi.Dtos.Category;
using BookShopApi.Models;

namespace BookShopApi.Interfaces
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<Category>> GetAllCategorysAsync();
        Task<Category?> GetCategoryByIdAsync(int id);
        Task<Category> CreateCategoryAsync(CreateCategoryDto categoryDto);
        Task<Category?> UpdateCategoryAsync(UpdateCategoryDto categoryDto, int id);
        Task<Category?> DeleteCategoryAsync(int id);
    }
}
