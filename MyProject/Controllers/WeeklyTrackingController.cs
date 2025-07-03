using Common.Dto;
using Microsoft.AspNetCore.Mvc;
using Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WeeklyTrackingController : ControllerBase
    {
        private readonly IService<WeeklyTrackingDto> _service;

        public WeeklyTrackingController(IService<WeeklyTrackingDto> service)
        {
            _service = service;
        }

        // קבלת כל הרשומות, עם אפשרות סינון לפי custId (מזהה לקוח)
        [HttpGet]
        public async Task<ActionResult<List<WeeklyTrackingDto>>> GetAll([FromQuery] int? custId = null)
        {
            var all = await _service.GetAllAsync();

            if (custId != null)
                all = all.Where(x => x.CustId == custId).ToList();

            return Ok(all);
        }

        // קבלת רשומה בודדת לפי מזהה רשומה (id)
        [HttpGet("{id}")]
        public async Task<ActionResult<WeeklyTrackingDto>> Get(int id)
        {
            try
            {
                var tracking = await _service.GetByIdAsync(id);
                if (tracking == null)
                    return NotFound($"WeeklyTracking with id {id} not found.");

                return Ok(tracking);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<ActionResult<WeeklyTrackingDto>> Post([FromBody] WeeklyTrackingDto weeklyTracking)
        {
            if (weeklyTracking == null)
                return BadRequest("Invalid weeklyTracking data.");

            try
            {
                var addedItem = await _service.AddItemAsync(weeklyTracking);
                return CreatedAtAction(nameof(Get), new { id = addedItem.Id }, addedItem);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] WeeklyTrackingDto weeklyTrackingDto)
        {
            if (id != weeklyTrackingDto.Id)
                return BadRequest("ID mismatch.");

            try
            {
                await _service.UpdateItemAsync(id, weeklyTrackingDto);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

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
    }
}
