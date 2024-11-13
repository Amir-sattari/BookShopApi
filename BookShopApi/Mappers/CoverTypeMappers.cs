using BookShopApi.Dtos.Category;
using BookShopApi.Dtos.CoverType;
using BookShopApi.Models;

namespace BookShopApi.Mappers
{
    public static class CoverTypeMappers
    {
        public static CoverTypeDto ToCoverTypeDto(this CoverType coverType)
        {
            return new CoverTypeDto
            {
                Id = coverType.Id,
                Name = coverType.Name,
            };
        }

        public static CoverType ToCoverTypeFromCreate(this CreateCoverTypeDto coverTypeDto)
        {
            return new CoverType
            {
                Name = coverTypeDto.Name,
            };
        }

        public static CoverType SetDataToCoverTypeFromCreateDto(this CreateCoverTypeDto coverTypeDto)
        {
            return new CoverType
            {
                Name = coverTypeDto.Name,
            };
        }

        public static void SetDataToCoverTypeFromUpdateDto(this CoverType coverType, UpdateCoverTypeDto coverTypeDto)
        {
            coverType.Name = coverTypeDto.Name;
        }
    }
}
