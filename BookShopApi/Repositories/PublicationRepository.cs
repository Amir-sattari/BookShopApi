using BookShopApi.Data;
using BookShopApi.Dtos.Publication;
using BookShopApi.Interfaces;
using BookShopApi.Mappers;
using BookShopApi.Models;
using Microsoft.EntityFrameworkCore;

namespace BookShopApi.Repositories
{
    public class PublicationRepository : IPublicationRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IFileService _fileService;

        public PublicationRepository(ApplicationDbContext applicationDbContext, IFileService fileService)
        {
            _context = applicationDbContext;
            _fileService = fileService;
        }

        public async Task<IEnumerable<Publication>> GetAllPublicationsAsync()
        {
            return await _context.Publications.ToListAsync();
        }

        public async Task<Publication?> GetPublicationByIdAsync(int id)
        {
            return await _context.Publications.FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Publication> CreatePublicationAsync(CreatePublicationDto publicationDto)
        {
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
            string imageUrl = await _fileService.SaveFileAsync(publicationDto.ImageFile, allowedExtensions, "Publication");

            var publication = publicationDto.SetDataToPublicationFromCreateDto(imageUrl);
            await _context.Publications.AddAsync(publication);
            await _context.SaveChangesAsync();
            return publication;
        }

        public async Task<Publication?> UpdatePublicationAsync(UpdatePublicationDto publicationDto, int id)
        {
            var publication = await _context.Publications.FirstOrDefaultAsync(p => p.Id == id);

            if (publication == null)
                return null;

            if (publicationDto.ImageFile != null)
            {
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
                string folderName = "Publication";

                if (!string.IsNullOrEmpty(publication.ImageUrl))
                    _fileService.DeleteFile(publication.ImageUrl, folderName);

                publication.ImageUrl = await _fileService.SaveFileAsync(publicationDto.ImageFile, allowedExtensions, folderName);
            }

            publication.SetDataToPublicationFromUpdateDto(publicationDto);

            await _context.SaveChangesAsync();
            return publication;
        }

        public async Task<Publication?> DeletePublicationAsync(int id)
        {
            var publication = await _context.Publications.FirstOrDefaultAsync(p => p.Id == id);

            if (publication == null)
                return null;

            _context.Remove(publication);
            await _context.SaveChangesAsync();
            return publication;
        }
    }
}
