using BookShopApi.Dtos.Province;
using BookShopApi.Models;

namespace BookShopApi.Interfaces
{
    public interface IProvinceRepository
    {
        Task<ICollection<Province>> GetProvincesAsync();
        Task<Province?> GetProvinceByIdAsync(int id);
        Task<Province> CreateProvinceAsync(CreateProvinceDto provinceDto);
        Task<Province?> UpdateProvinceAsync(UpdateProvinceDto provinceDto, int id);
        Task<Province?> DeleteProvinceAsync(int id);
    }
}
