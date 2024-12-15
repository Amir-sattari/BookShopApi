using BookShopApi.Dtos.ShippingMethod;
using BookShopApi.Models;

namespace BookShopApi.Interfaces
{
    public interface IShippingMethodRepository
    {
        Task<ICollection<ShippingMethod>> GetAllShippingMethodsAsync();
        Task<ShippingMethod?> GetShippingMethodByIdAsync(int id);
        Task<ShippingMethod> CreateShippingMethodAsync(CreateShippingMethodDto methodDto);
        Task<ShippingMethod?> UpdateShippingMethodAsync(UpdateShippingMethodDto methodDto, int id);
        Task<ShippingMethod?> DeleteShippingMethodAsync(int id);
    }
}
