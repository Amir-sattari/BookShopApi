using BookShopApi.Dtos.Publication;
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

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Publication>> GetPublicationByIdAsync([FromRoute] int id)
        {
            var publication = await _publicationRepo.GetPublicationByIdAsync(id);

            if (publication == null)
                return NotFound($"The publication with Id: {id}, Not found.");

            return Ok(publication.ToPublicationDto());
        }

        [HttpPost]
        public async Task<ActionResult<Publication>> CreatePublicationAsync([FromBody] CreatePublicationDto publicationDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var publication = publicationDto.ToPublicationFromCreateDto();
            await _publicationRepo.CreatePublicationAsync(publication);
            return Created($"api/publication/{publication.Id}", publication.ToPublicationDto());
        }


        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdatePublicationAsync([FromBody] UpdatePublicationDto publicationDto, [FromRoute] int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);


            var publication = await _publicationRepo.UpdatePublicationAsync(publicationDto, id);

            if (publication == null)
                return NotFound($"The publication with Id: {id}, Not found.");

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeletePublicationAsync([FromRoute] int id)
        {
            var publication = await _publicationRepo.DeletePublicationAsync(id);

            if (publication == null)
                return NotFound($"The publication with Id: {id}, Not found.");

            return NoContent();
        }
    }
}
