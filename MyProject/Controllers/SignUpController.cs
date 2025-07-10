using Common.Dto;
using Microsoft.AspNetCore.Mvc;
using Repository.Entities; // ודא שקיים
using Service.Interfaces;
using System.IO; // ודא שקיים (אם כי לא נשתמש בו לשמירה ישירה)
using System.Threading.Tasks;

namespace MyProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SignUpController : ControllerBase
    {
        private readonly IService<CustomerDto> _service;

        public SignUpController(IService<CustomerDto> service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromForm] SignUpDto signUpDto)
        {
            if (signUpDto == null)
                return BadRequest("Invalid sign-up data.");

            if (signUpDto.Role == eRole.USER)
            {
                if (signUpDto.Height == null || signUpDto.Weight == null || signUpDto.Height <= 0 || signUpDto.Weight <= 0)
                    return BadRequest("Users must provide valid height and weight.");
            }
            else
            {
                signUpDto.Height = 0;
                signUpDto.Weight = 0;
            }

            var customer = new CustomerDto
            {
                FullName = signUpDto.FullName,
                Phone = signUpDto.Phone,
                Role = signUpDto.Role,
                Password = signUpDto.Password,
                Email = signUpDto.Email,
                Height = signUpDto.Height,
                Weight = signUpDto.Weight,
                // ImagePath ו-ImageUrl לא מוגדרים כאן, כי הם יטופלו על ידי ה-Service
                // ה-FileImage מועבר ישירות ל-Service שידאג להמרתו ל-byte[] ולשמירתו ב-DB.
                FileImage = signUpDto.FileImage, // מעבירים את ה-IFormFile ישירות!
                Gender = signUpDto.Gender,
                LikedProductIds = signUpDto.LikedProductIds,
                //DislikedProductIds = signUpDto.DislikedProductIds
            };

            var addedCustomer = await _service.AddItemAsync(customer);
            if (addedCustomer == null)
                return StatusCode(500, "Failed to add customer.");

            // הוספה של CreatedAtAction במקום Ok פשוט כדי להיות RESTful יותר
            return CreatedAtAction(nameof(CustomerController.Get), "Customer", new { id = addedCustomer.Id }, addedCustomer);
        }
    }
}
