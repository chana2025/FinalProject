using Common.Dto;
using Microsoft.AspNetCore.Mvc;
using Repository.Entities;
using Service.Interfaces;
using System.IO;
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

            byte[]? imageBytes = null;
            if (signUpDto.FileImage != null && signUpDto.FileImage.Length > 0)
            {
                string imageDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Images");
                if (!Directory.Exists(imageDirectory))
                    Directory.CreateDirectory(imageDirectory);

                string imagePath = Path.Combine(imageDirectory, $"{signUpDto.FullName}.jpg");

                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    await signUpDto.FileImage.CopyToAsync(stream);
                }

                using (var ms = new MemoryStream())
                {
                    await signUpDto.FileImage.CopyToAsync(ms);
                    imageBytes = ms.ToArray();
                }
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
                ImagePath = imageBytes,
                ImageUrl = $"{signUpDto.FullName}.jpg",
                Gender = signUpDto.Gender,
                LikedProductIds = signUpDto.LikedProductIds,
                DislikedProductIds = signUpDto.DislikedProductIds
            };

            var addedCustomer = await _service.AddItemAsync(customer);
            if (addedCustomer == null)
                return StatusCode(500, "Failed to add customer.");

            return Ok("Customer added successfully.");
        }
    }
}
