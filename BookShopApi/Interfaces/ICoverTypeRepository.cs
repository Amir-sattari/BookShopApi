using BookShopApi.Dtos.CoverType;
using BookShopApi.Models;

namespace BookShopApi.Interfaces
{
    public interface ICoverTypeRepository
    {
        Task<IEnumerable<CoverType>> GetAllCoverTypesAsync();
        Task<CoverType?> GetCoverTypeByIdAsync(int id);
        Task<CoverType> CreateCoverTypeAsync(CreateCoverTypeDto bocoverTypeok);
        Task<CoverType?> UpdateCoverTypeAsync(UpdateCoverTypeDto coverTypeDto, int id);
        Task<CoverType?> DeleteCoverTypeAsync(int id);
    }
}
