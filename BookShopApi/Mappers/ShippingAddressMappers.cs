using BookShopApi.Dtos.ShippingAddress;
using BookShopApi.Models;
using System.Net;

namespace BookShopApi.Mappers
{
    public static class ShippingAddressMappers
    {
        public static ShippingAddressDto ToShippingAddressDto(this ShippingAddress address)
        {
            return new ShippingAddressDto
            {
                Id = address.Id,
                UserId = address.UserId,
                UserName = address.UserName,
                PhoneNumber = address.PhoneNumber,
                ProvinceId = address.ProvinceId,
                CityId = address.CityId,
                Address = address.Address,
                PostCode = address.PostCode,
            };
        }

        public static ShippingAddress SetDataToShippingAddressFromCreateDto(this CreateShippingAddressDto addressDto)
        {
            return new ShippingAddress
            {
                UserId = addressDto.UserId,
                UserName = addressDto.UserName,
                PhoneNumber = addressDto.PhoneNumber,
                ProvinceId = addressDto.ProvinceId,
                CityId = addressDto.CityId,
                Address = addressDto.Address,
                PostCode = addressDto.PostCode,
            };
        }

        public static void SetDataToShippingAddressFromUpdateDto(this ShippingAddress shippingAddress, UpdateShippingAddressDto addressDto, string userId)
        {
            shippingAddress.UserId = userId;
            shippingAddress.UserName = addressDto.UserName;
            shippingAddress.PhoneNumber = addressDto.PhoneNumber;
            shippingAddress.ProvinceId = addressDto.ProvinceId;
            shippingAddress.CityId = addressDto.CityId;
            shippingAddress.Address = addressDto.Address;
            shippingAddress.PostCode = addressDto.PostCode;
        }
    }
}
