using BookShopApi.Data;
using BookShopApi.Dtos;
using BookShopApi.Interfaces;
using BookShopApi.Models;
using Microsoft.EntityFrameworkCore;

namespace BookShopApi.Repositories
{
    public class PublicationRepository : IPublicationRepository
    {
        private readonly ApplicationDbContext _context;

        public PublicationRepository(ApplicationDbContext applicationDbContext)
        {
            _context = applicationDbContext;
        }

        public async Task<ICollection<Publication>> GetAllPublicationsAsync()
        {
            return await _context.Publications.ToListAsync();
        }

        public async Task<Publication?> GetPublicationByIdAsync(int id)
        {
            return await _context.Publications.FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Publication> CreatePublicationAsync(Publication publication)
        {
            await _context.Publications.AddAsync(publication);
            await _context.SaveChangesAsync();
            return publication;
        }

        public async Task<Publication?> UpdatePublicationAsync(UpdatePublicationDto publicationDto, int id)
        {
            var publication = await _context.Publications.FirstOrDefaultAsync(p => p.Id == id);

            if (publication == null)
                return null;

            publication.Name = publicationDto.Name;
            publication.ImageUrl = publicationDto.ImageUrl;

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
