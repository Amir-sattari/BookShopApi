using BookShopApi.Dtos.City;
using BookShopApi.Models;

namespace BookShopApi.Interfaces
{
    public interface ICityRepository
    {
        Task<ICollection<City>> GetcitiesAsync(int pageNumber, int pageSize);
        Task<City?> GetCityByIdAsync(int id);
        Task<ICollection<City>> GetCitiesByProvinceId(int provinceId);
        Task<City> CreateCityAsync(CreateCityDto cityDto);
        Task<City?> UpdateCityAsync(UpdateCityDto cityDto, int id);
        Task<City?> DeleteCityAsync(int id);
    }
}
