using BookShopApi.Dtos;
using BookShopApi.Models;

namespace BookShopApi.Mappers
{
    public static class publicatinMappers
    {
        public static PublicationDto ToPublicationDto(this Publication publication)
        {
            return new PublicationDto
            {
                Id = publication.Id,
                Name = publication.Name,
                ImageUrl = publication.ImageUrl,
            };
        }

        public static Publication ToPublicationFromCreateDto(this CreatePublicationDto publicationDto)
        {
            return new Publication
            {
                Name = publicationDto.Name,
                ImageUrl = publicationDto.ImageUrl,
            };
        }

        public static Publication ToPublicationFromUpdateDto(this UpdatePublicationDto publicationDto)
        {
            return new Publication
            {
                Name = publicationDto.Name,
                ImageUrl = publicationDto.ImageUrl,
            };
        }
    }
}
