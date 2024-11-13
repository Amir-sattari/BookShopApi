using BookShopApi.Dtos.Publication;
using BookShopApi.Models;

namespace BookShopApi.Interfaces
{
    public interface IPublicationRepository
    {
        Task<IEnumerable<Publication>> GetAllPublicationsAsync();
        Task<Publication?> GetPublicationByIdAsync(int id);
        Task<Publication> CreatePublicationAsync(CreatePublicationDto publicationDto);
        Task<Publication?> UpdatePublicationAsync(UpdatePublicationDto publicationDto, int id);
        Task<Publication?> DeletePublicationAsync(int id);
    }
}
