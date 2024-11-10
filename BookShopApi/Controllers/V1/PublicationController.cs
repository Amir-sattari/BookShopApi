using BookShopApi.Interfaces;
using BookShopApi.Mappers;
using BookShopApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace BookShopApi.Controllers.V1
{
    [Route("api/[controller]")]
    [ApiController]
    public class PublicationController : ControllerBase
    {
        private readonly IPublicationRepository _publicationRepo;

        public PublicationController(IPublicationRepository publicationRepository)
        {
            _publicationRepo = publicationRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Publication>>> GetAllPublicationsAsync()
        {
            var publications = await _publicationRepo.GetAllPublicationsAsync();
            var publicationsDto = publications.Select(p => p.ToPublicationDto()).ToList();
            return Ok(publicationsDto);
        }
    }
}
