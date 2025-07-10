using Common.Dto;
using Microsoft.AspNetCore.Mvc;
using Service.Interfaces;
using System.Threading.Tasks;

namespace MyProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerDietController : ControllerBase
    {
        private readonly ICustomerDietService _service;

        public CustomerDietController(ICustomerDietService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> AssignDietToCustomer([FromBody] CustomerDietDto dto)
        {
            await _service.AddAsync(dto);
            return Ok("Diet assigned to customer.");
        }

        [HttpGet("{customerId}")]
        public async Task<IActionResult> GetDietByCustomer(int customerId)
        {
            var diet = await _service.GetDietByCustomerIdAsync(customerId);
            if (diet == null)
                return NotFound("Customer has no diet assigned.");
            return Ok(diet);
        }
    }
}
