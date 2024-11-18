using BookShopApi.Dtos.CoverType;
using BookShopApi.Dtos.Publication;
using BookShopApi.Models;

namespace BookShopApi.Mappers
{
    public static class publicatinMappers
    {
        public static PublicationDto ToPublicationDto(this Publication publication, string imageUrl)
        {
            return new PublicationDto
            {
                Id = publication.Id,
                Name = publication.Name,
                ImageUrl = imageUrl,
            };
        }

        public static Publication ToPublicationFromCreateDto(this CreatePublicationDto publicationDto, string imageUrl)
        {
            return new Publication
            {
                Name = publicationDto.Name,
                ImageUrl = imageUrl,
            };
        }

        public static Publication ToPublicationFromUpdateDto(this UpdatePublicationDto publicationDto, string imageUrl)
        {
            return new Publication
            {
                Name = publicationDto.Name,
                ImageUrl = imageUrl,
            };
        }

        public static Publication SetDataToPublicationFromCreateDto(this CreatePublicationDto publicationDto, string imageUrl)
        {
            return new Publication
            {
                Name = publicationDto.Name,
                ImageUrl = imageUrl
            };
        }

        public static void SetDataToPublicationFromUpdateDto(this Publication publication, UpdatePublicationDto publicationDto)
        {
            publication.Name = publicationDto.Name;
        }
    }
}
