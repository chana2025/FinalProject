using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Common.Dto;
using Service.Interfaces;
using System;
using System.Threading.Tasks;

namespace MyProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FoodPreferenceController : ControllerBase
    {
        private readonly IFoodPreferenceService _foodPreferenceService;

        public FoodPreferenceController(IFoodPreferenceService foodPreferenceService)
        {
            _foodPreferenceService = foodPreferenceService;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> SavePreferences([FromForm] FoodPreferencesDto dto)
        {
            var userIdClaim = User.FindFirst("id");
            if (userIdClaim == null)
                return Unauthorized("User ID not found in token");

            try
            {
                int userId = int.Parse(userIdClaim.Value);
                await _foodPreferenceService.SaveUserPreferencesAsync(dto, userId);
                return Ok("Preferences saved");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetPreferences()
        {
            var userIdClaim = User.FindFirst("id");
            if (userIdClaim == null)
                return Unauthorized("User ID not found in token");

            try
            {
                int userId = int.Parse(userIdClaim.Value);
                var result = await _foodPreferenceService.GetUserPreferencesAsync(userId);
                if (result == null)
                    return NotFound("Preferences not found");

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
