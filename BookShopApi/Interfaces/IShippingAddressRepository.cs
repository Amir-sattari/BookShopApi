using BookShopApi.Dtos.ShippingAddress;
using BookShopApi.Models;

namespace BookShopApi.Interfaces
{
    public interface IShippingAddressRepository
    {
        Task<ICollection<ShippingAddress>> GetAllShippingAddressesAsync();
        Task<ShippingAddress?> GetShippingAddressByUserIdAsync(string userId);
        Task<ShippingAddress> CreateShippingAddressAsync(CreateShippingAddressDto addressDto);
        Task<ShippingAddress?> UpdateShippingAddressAsync(UpdateShippingAddressDto addressDto, string userId);
        Task<ShippingAddress?> DeleteShippingAddressByUserIdAsync(string userId);
    }
}
