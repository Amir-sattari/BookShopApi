using BookShopApi.Dtos.City;
using BookShopApi.Interfaces;
using BookShopApi.Mappers;
using BookShopApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace BookShopApi.Controllers.V1
{
    [Route("api/[controller]")]
    [ApiController]
    public class CityController : ControllerBase
    {
        private readonly ICityRepository _cityRepo;

        public CityController(ICityRepository cityRepository)
        {
            _cityRepo = cityRepository;
        }

        [HttpGet("GetCities")]
        public async Task<ActionResult<IEnumerable<City>>> GetCitiesAsync()
        {
            var cities = await _cityRepo.GetcitiesAsync();
            var toCitiesDto = cities.Select(c => c.ToCityDto()).ToList();
            return Ok(toCitiesDto);
        }

        [HttpGet("GetCityById/{id:int}")]
        public async Task<ActionResult<City>> GetCityByIdAsync([FromRoute] int id)
        {
            var city = await _cityRepo.GetCityByIdAsync(id);

            if(city == null)
                return NotFound(new { Message = "City Not Found." });

            return Ok(city.ToCityDto());
        }

        [HttpGet("GetCitiesByProvinceId/{id:int}")]
        public async Task<ActionResult<IEnumerable<City>>> GetCitiesByProvinceId([FromRoute] int id)
        {
            var cities = await _cityRepo.GetCitiesByProvinceId(id);
            var toCitiesDto = cities.Select(c => c.ToCityDto()).ToList();
            return Ok(toCitiesDto);
        }

        [HttpPost("CreateCity")]
        public async Task<ActionResult<City>> CreateCityAsync([FromBody] CreateCityDto cityDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var createdCity = await _cityRepo.CreateCityAsync(cityDto);
                return Created($"api/city/{createdCity.Id}", createdCity.ToCityDto());
            }
            catch (InvalidOperationException e)
            {
                return BadRequest(e.Message);
            }
            catch (Exception e)
            {
                return BadRequest(new { Message = "An Unexpected Error Occurred."});
            }

        }

        [HttpPut("UpdateCity/{id:int}")]
        public async Task<IActionResult> UpdateCityAsync([FromBody] UpdateCityDto cityDto, [FromRoute] int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var updatedCity = await _cityRepo.UpdateCityAsync(cityDto, id);

                if (updatedCity == null)
                    return NotFound(new { Message = "City Not Found." });

                return NoContent();
            }
            catch (InvalidOperationException e)
            {
                return BadRequest(e.Message);
            }
            catch (Exception e)
            {
                return BadRequest(new { Message = "An Unexpected Error Occurred." });
            }

        }

        [HttpDelete("DeleteCity/{id:int}")]
        public async Task<IActionResult> DeleteCityAsync([FromRoute] int id)
        {
            var city = await _cityRepo.DeleteCityAsync(id);

            if (city == null)
                return NotFound(new { Message = "City Not Found." });

            return NoContent();
        }
    }
}
