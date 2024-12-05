using BookShopApi.Dtos.City;
using BookShopApi.Models;

namespace BookShopApi.Mappers
{
    public static class CityMappers
    {
        public static CityDto ToCityDto(this City city)
        {
            return new CityDto
            {
                Id = city.Id,
                Name = city.Name,
                ProvinceId = city.ProvinceId,
            };
        }

        public static City SetDataToCityFromCreateDto(this CreateCityDto cityDto)
        {
            return new City
            {
                Name = cityDto.Name,
                ProvinceId = cityDto.ProvinceId,
            };
        }
        public static void SetDataToCityFromUpdateDto(this City city, UpdateCityDto cityDto)
        {
            city.Name = cityDto.Name;
            city.ProvinceId = cityDto.ProvinceId;
        }
    }
}
