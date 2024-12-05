using BookShopApi.Dtos.Province;
using BookShopApi.Models;

namespace BookShopApi.Mappers
{
    public static class ProvinceMappers
    {
        public static ProvinceDto ToProvinceDto(this Province province)
        {
            return new ProvinceDto
            {
                Id = province.Id,
                Name = province.Name,
            };
        }

        public static Province SetDataToProvinceFromCreateDto(this CreateProvinceDto provinceDto)
        {
            return new Province
            {
                Name = provinceDto.Name,
            };
        }

        public static void SetDataToProvinceFromUpdateDto(this Province province, UpdateProvinceDto provinceDto)
        {
            province.Name = provinceDto.Name;
        }
    }
}
