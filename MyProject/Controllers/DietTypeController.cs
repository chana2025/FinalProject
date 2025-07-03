using Common.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Interfaces;
using Service.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DietTypeController : ControllerBase
    {
        private readonly IService<DietDto> _service;
        private readonly IFileUploadService _fileUploadService;
        private readonly GreedyAlg _greedyAlg;

        public DietTypeController(IFileUploadService fileUploadService, IService<DietDto> service, GreedyAlg greedyAlg)
        {
            _fileUploadService = fileUploadService;
            _service = service;
            _greedyAlg = greedyAlg;
        }

        // GET: api/DietType
        [HttpGet]
        public async Task<ActionResult<List<DietDto>>> Get()
        {
            try
            {
                var diets = await _service.GetAllAsync();
                return Ok(diets);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // GET: api/DietType/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DietDto>> Get(int id)
        {
            try
            {
                var diet = await _service.GetByIdAsync(id);
                if (diet == null)
                    return NotFound($"Diet with id {id} not found.");

                return Ok(diet);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // POST: api/DietType
        [HttpPost]
        public async Task<IActionResult> Post([FromForm] DietDto diet)
        {
            if (diet == null)
                return BadRequest("Invalid Diet data.");

            try
            {
                if (diet.fileImage != null && diet.fileImage.Length > 0)
                {
                    var imagePath = await _fileUploadService.UploadImageAsync(diet.fileImage, "dietTypes");
                    diet.ImageUrl = imagePath;
                }

                await _service.AddItemAsync(diet);
                return Ok("Diet added successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // PUT: api/DietType/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromForm] DietDto dietDto)
        {
            if (id != dietDto.DietId)
                return BadRequest("ID mismatch.");

            try
            {
                await _service.UpdateItemAsync(id, dietDto);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // DELETE: api/DietType/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _service.DeleteItemAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // פונקציה חדשה - התאמה חמדנית של דיאטה ללקוח
        [HttpGet("match-diet/{customerId}")]
        public IActionResult MatchDietToCustomer(int customerId)
        {
            try
            {
                var matchedDiet = _greedyAlg.MatchBestDietsForCustomer(customerId);
                if (matchedDiet == null)
                    return NotFound("No suitable diet found for this customer.");

                return Ok(matchedDiet);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
