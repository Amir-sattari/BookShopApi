using BookShopApi.Dtos;
using BookShopApi.Models;

namespace BookShopApi.Interfaces
{
    public interface IPublicationRepository
    {
        Task<ICollection<Publication>> GetAllPublicationsAsync();
        Task<Publication?> GetPublicationByIdAsync(int id);
        Task<Publication> CreatePublicationAsync(Publication publicationDto);
        Task<Publication?> UpdatePublicationAsync(Publication publicationDto, int id);
        Task<Publication?> DeletePublicationAsync(int id);
    }
}
