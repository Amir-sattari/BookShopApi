using BookShopApi.Dtos.CoverType;
using BookShopApi.Interfaces;
using BookShopApi.Mappers;
using BookShopApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace BookShopApi.Controllers.V1
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoverTypeController : ControllerBase
    {
        private readonly ICoverTypeRepository _coverTypeRepo;

        public CoverTypeController(ICoverTypeRepository coverTypeRepository)
        {
            _coverTypeRepo = coverTypeRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CoverType>>> GetAllCoverTypesAsync()
        {
            var coverTypes = await _coverTypeRepo.GetAllCoverTypesAsync();
            var coverTypesDto = coverTypes.Select(c => c.ToCoverTypeDto()).ToList();
            return Ok(coverTypesDto);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<CoverType>> GetCoverTypeByIdAsync([FromRoute] int id)
        {
            var coverType = await _coverTypeRepo.GetCoverTypeByIdAsync(id);

            if (coverType == null)
                return NotFound($"The CoverType with Id: {id}, Not found.");

            return Ok(coverType.ToCoverTypeDto());
        }

        [HttpPost]
        public async Task<ActionResult<CoverType>> CreateCoverTypeAsync(CreateCoverTypeDto coverTypeDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var createdCoverType = await _coverTypeRepo.CreateCoverTypeAsync(coverTypeDto);
            return Created($"api/coverType/{createdCoverType.Id}", createdCoverType.ToCoverTypeDto());
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateCoverTypeAsync([FromBody] UpdateCoverTypeDto coverTypeDto, [FromRoute] int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var coverType = await _coverTypeRepo.UpdateCoverTypeAsync(coverTypeDto, id);

            if (coverType == null)
                return NotFound($"The CoverType with Id: {id}, Not found.");

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteCoverTypeAsync([FromRoute] int id)
        {
            var coverType = await _coverTypeRepo.DeleteCoverTypeAsync(id);

            if (coverType == null)
                return NotFound($"The CoverType with Id: {id}, Not found.");

            return NoContent();
        }
    }
}
