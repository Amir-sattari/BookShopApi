using BookShopApi.Data;
using BookShopApi.Dtos.Category;
using BookShopApi.Interfaces;
using BookShopApi.Mappers;
using BookShopApi.Models;
using Microsoft.EntityFrameworkCore;

namespace BookShopApi.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext _context;

        public CategoryRepository(ApplicationDbContext applicationDbContext)
        {
            _context = applicationDbContext;
        }


        public async Task<IEnumerable<Category>> GetAllCategorysAsync()
        {
            return await _context.Categories.ToListAsync();
        }

        public async Task<Category?> GetCategoryByIdAsync(int id)
        {
            return await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Category> CreateCategoryAsync(CreateCategoryDto categoryDto)
        {
            var category = categoryDto.SetDataToCategoryFromCreateDto();

            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();
            return category;
        }

        public async Task<Category?> UpdateCategoryAsync(UpdateCategoryDto categoryDto, int id)
        {
            var category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);

            if (category == null)
                return null;

            category.SetDataToCategoryFromUpdateDto(categoryDto);

            await _context.SaveChangesAsync();
            return category;
        }

        public async Task<Category?> DeleteCategoryAsync(int id)
        {
            var category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);

            if (category == null)
                return null;

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            return category;
        }
    }
}
