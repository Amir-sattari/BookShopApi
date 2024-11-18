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
            var publicationsDto = publications.Select(p => p.ToPublicationDto($"{Request.Scheme}://{Request.Host}{p.ImageUrl}")).ToList();
            return Ok(publicationsDto);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Publication>> GetPublicationByIdAsync([FromRoute] int id)
        {
            var publication = await _publicationRepo.GetPublicationByIdAsync(id);

            if (publication == null)
                return NotFound($"The publication with Id: {id}, Not found.");

            return Ok(publication.ToPublicationDto($"{Request.Scheme}://{Request.Host}{publication.ImageUrl}"));
        }

        [HttpPost]
        public async Task<ActionResult<Publication>> CreatePublicationAsync([FromForm] CreatePublicationDto publicationDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var createdPublication = await _publicationRepo.CreatePublicationAsync(publicationDto);
            return Created($"api/publication/{createdPublication.Id}", createdPublication.ToPublicationDto($"{Request.Scheme}://{Request.Host}{createdPublication.ImageUrl}"));
        }


        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdatePublicationAsync([FromForm] UpdatePublicationDto publicationDto, [FromRoute] int id)
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
