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
        private readonly IFileService _fileService;

        public CategoryRepository(ApplicationDbContext applicationDbContext, IFileService fileService)
        {
            _context = applicationDbContext;
            _fileService = fileService;
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

            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
            var imageUrl = await _fileService.SaveFileAsync(categoryDto.ImageFile, allowedExtensions, "Category");

            var category = categoryDto.SetDataToCategoryFromCreateDto(imageUrl);

            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();
            return category;
        }

        public async Task<Category?> UpdateCategoryAsync(UpdateCategoryDto categoryDto, int id)
        {
            var category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);

            if (category == null)
                return null;

            if (categoryDto.ImageFile != null)
            {
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
                string folderName = "Category";
                if (!string.IsNullOrEmpty(category.ImageUrl))
                    _fileService.DeleteFile(category.ImageUrl, folderName);

                category.ImageUrl = await _fileService.SaveFileAsync(categoryDto.ImageFile, allowedExtensions, "Category");
            }

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
