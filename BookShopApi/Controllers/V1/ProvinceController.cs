using BookShopApi.Dtos.Province;
using BookShopApi.Interfaces;
using BookShopApi.Mappers;
using BookShopApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace BookShopApi.Controllers.V1
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProvinceController : ControllerBase
    {
        private readonly IProvinceRepository _provinceRepo;

        public ProvinceController(IProvinceRepository provinceRepository)
        {
            _provinceRepo = provinceRepository;
        }

        [HttpGet("GetProvinces")]
        public async Task<ActionResult<IEnumerable<Province>>> GetProvincesAsync()
        {
            var provinces = await _provinceRepo.GetProvincesAsync();
            var toProvinceDto = provinces.Select(p => p.ToProvinceDto()).ToList();
            return Ok(toProvinceDto);
        }

        [HttpGet("GetProvinceById/{id:int}")]
        public async Task<ActionResult<Province>> GetProvinceByIdAsync([FromRoute] int id)
        {
            var province = await _provinceRepo.GetProvinceByIdAsync(id);

            if (province == null)
                return NotFound(new { Message = "Province Not Found." });

            return Ok(province.ToProvinceDto());
        }

        [HttpPost("CreateProvince")]
        public async Task<ActionResult<Province>> CreateProvinceAsync([FromBody] CreateProvinceDto provinceDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var createdProvince = await _provinceRepo.CreateProvinceAsync(provinceDto);
            return Created($"api/province/{createdProvince.Id}", createdProvince.ToProvinceDto());
        }

        [HttpPut("UpdateProvince/{id:int}")]
        public async Task<IActionResult> UpdateProvinceAsync([FromBody] UpdateProvinceDto provinceDto, [FromRoute] int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var updatedProvince = await _provinceRepo.UpdateProvinceAsync(provinceDto, id);

            if (updatedProvince == null)
                return NotFound(new { Message = "Province Not Found." });

            return NoContent();
        }

        [HttpDelete("Deleteprovince/{id:int}")]
        public async Task<IActionResult> DeleteProvinceAsync([FromRoute] int id)
        {
            var province = await _provinceRepo.DeleteProvinceAsync(id);

            if (province == null)
                return NotFound(new { Message = "Province Not Found." });

            return NoContent();
        }
    }
}
