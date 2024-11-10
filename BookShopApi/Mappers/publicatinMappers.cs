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
                Name = publication.Name,
                ImageUrl = publication.ImageUrl,
            };
        }
    }
}
