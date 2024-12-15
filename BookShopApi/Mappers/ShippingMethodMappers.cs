using BookShopApi.Dtos.ShippingMethod;
using BookShopApi.Models;

namespace BookShopApi.Mappers
{
    public static class ShippingMethodMappers
    {
        public static ShippingMethodDto ToShippingMethodDto(this ShippingMethod shippingMethod)
        {
            return new ShippingMethodDto
            {
                Id = shippingMethod.Id,
                Name = shippingMethod.Name,
                Cost = shippingMethod.Cost,
            };
        }

        public static ShippingMethod SetDataToShippingMethodFromCreateDto(this CreateShippingMethodDto methodDto)
        {
            return new ShippingMethod
            {
                Name = methodDto.Name,
                Cost = methodDto.Cost,
            };
        }

        public static void SetDataToShippingMethodFromUpdateDto(this ShippingMethod shippingMethod, UpdateShippingMethodDto methodDto)
        {
            shippingMethod.Name = methodDto.Name;
            shippingMethod.Cost = methodDto.Cost;
        }
    }
}
