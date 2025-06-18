using Common.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Interfaces;
using Service.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MyProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DietTypeController : ControllerBase
    {
        private readonly IService<DietDto> _service;
        private readonly IFileUploadService _fileUploadService;

        public DietTypeController(IFileUploadService fileUploadService, IService<DietDto> _service)
        {
            _fileUploadService = fileUploadService;
            this._service = _service;
        }


        // GET: api/<DietTypeController>

        [HttpGet]
        public ActionResult<List<DietDto>> Get()
        {
            try
            {
                var diets = _service.GetAll();
                return Ok(diets);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        //V
        [HttpGet("{id}")]
        public ActionResult<DietDto> Get(int id)
        {
            try
            {
                var diet = _service.GetById(id);
                if (diet == null)
                    return NotFound($"diet with id {id} not found.");

                return Ok(diet);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }









        [HttpPost]
        public async Task<IActionResult> Post([FromForm] DietDto diet)
        {
            if (diet == null)
                return BadRequest("Invalid Diet data.");

            if (diet.fileImage != null && diet.fileImage.Length > 0)
            {
                // מעלים את התמונה לתיקיית "dietTypes" ומקבלים את הנתיב
                var imagePath = await _fileUploadService.UploadImageAsync(diet.fileImage, "dietTypes");

                // שומרים את הנתיב בשדה המתאים
                diet.ImageUrl = imagePath;
            }

            _service.AddItem(diet);
            return Ok("Diet added successfully.");
        }




        // PUT api/<DietTypeController>/5
        [HttpPut("{id}")]

        public IActionResult Put(int id, [FromForm] DietDto dietDto)
        {
            if (id != dietDto.DietId)
                return BadRequest("ID mismatch.");

            try
            {
                _service.UpdateItem(id, dietDto);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        // DELETE api/<DietTypeController>/5
        [HttpDelete("{id}")]

        public IActionResult Delete(int id)
        {
            try
            {
                _service.DeleteItem(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

    }
}