//using Common.Dto;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using Service.Interfaces;
//using System.Security.Claims;
//using System.Threading.Tasks;
//using System.IO;
//using System;
//using Repository.Entities;
//using Service.Services;
//using Microsoft.AspNetCore.Http;
//using System.Collections.Generic;

//namespace MyProject.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class CustomerController : ControllerBase
//    {
//        private readonly IService<CustomerDto> _service;
//        private readonly IFileUploadService _fileUploadService;

//        public CustomerController(IService<CustomerDto> service, IFileUploadService fileUploadService)
//        {
//            _service = service;
//            _fileUploadService = fileUploadService;
//        }

//        // שליפת כל הלקוחות - אסינכרונית
//        [HttpGet]
//        [Authorize(Roles = "ADMIN, WORKER")]
//        public async Task<ActionResult<List<CustomerDto>>> Get()
//        {
//            try
//            {
//                var customers = await _service.GetAllAsync();
//                return Ok(customers);
//            }
//            catch (Exception ex)
//            {
//                return StatusCode(500, $"Internal server error: {ex.Message}");
//            }
//        }

//        // שליפת לקוח לפי מזהה (int id) - אסינכרונית
//        [HttpGet("{id}")]
//        [Authorize]
//        public async Task<ActionResult<CustomerDto>> Get(int id)
//        {
//            try
//            {
//                var customer = await _service.GetByIdAsync(id);
//                if (customer == null)
//                    return NotFound($"Customer with id {id} not found.");

//                return Ok(customer);
//            }
//            catch (Exception ex)
//            {
//                return StatusCode(500, $"Internal server error: {ex.Message}");
//            }
//        }

//        // הוספת לקוח חדש (כולל אפשרות להעלות תמונה) - אסינכרונית
//        //[HttpPost]
//        //[Authorize(Roles = "ADMIN")]
//        //public async Task<ActionResult<CustomerDto>> Post([FromForm] CustomerCreateRequest request)
//        //{
//        //    if (request == null)
//        //        return BadRequest("Invalid request.");

//        //    string imagePath = null;
//        //    if (request.Image != null && request.Image.Length > 0)
//        //    {
//        //       imagePath = await _fileUploadService.UploadImageAsync(request.Image, "dietTypes");
//        //    }

//        //    var customerDto = new CustomerDto
//        //    {
//        //        Id = request.Id,
//        //        FullName = request.FullName,
//        //        Email = request.Email,
//        //        Phone = request.Phone,
//        //        Height = request.Height,
//        //        Weight = request.Weight,
//        //        Role = request.Role,
//        //        ImagePath = imagePath
//        //    };

//        //    var createdCustomer = await _service.AddItemAsync(customerDto);
//        //    return CreatedAtAction(nameof(Get), new { id = createdCustomer.Id }, createdCustomer);
//        //}

//        // עדכון לקוח קיים - אסינכרוני
//        [HttpPut("{id}")]
//        [Authorize(Roles = "ADMIN")]
//        public async Task<IActionResult> Put(int id, [FromForm] CustomerDto customerDto)
//        {
//            if (id != customerDto.Id)
//                return BadRequest("ID mismatch.");

//            try
//            {
//                await _service.UpdateItemAsync(id, customerDto);
//                return NoContent();
//            }
//            catch (Exception ex)
//            {
//                return StatusCode(500, $"Internal server error: {ex.Message}");
//            }
//        }

//        // מחיקת לקוח - אסינכרוני
//        [HttpDelete("{id}")]
//        [Authorize(Roles = "ADMIN")]
//        public async Task<IActionResult> Delete(int id)
//        {
//            try
//            {
//                await _service.DeleteItemAsync(id);
//                return NoContent();
//            }
//            catch (Exception ex)
//            {
//                return StatusCode(500, $"Internal server error: {ex.Message}");
//            }
//        }

//        [HttpGet("image/{id}")]
//        public async Task<IActionResult> GetCustomerImage(int id)
//        {
//            var customer = await _service.GetByIdAsync(id);
//            if (customer == null || customer.ImagePath == null)
//                return NotFound();

//            string mimeType = "image/jpeg"; // ברירת מחדל
//            if (!string.IsNullOrEmpty(customer.ImageUrl))
//            {
//                if (customer.ImageUrl.EndsWith(".png", StringComparison.OrdinalIgnoreCase))
//                    mimeType = "image/png";
//                else if (customer.ImageUrl.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase) ||
//                         customer.ImageUrl.EndsWith(".jpeg", StringComparison.OrdinalIgnoreCase))
//                    mimeType = "image/jpeg";
//            }

//            return File(customer.ImagePath, mimeType);
//        }
//    }
//}
using Common.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Interfaces;
using System.Security.Claims;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Http; // ודא שיש
using System.Collections.Generic;

namespace MyProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly IService<CustomerDto> _service;
        // private readonly IFileUploadService _fileUploadService; // כבר לא נחוץ אם התמונות נשמרות ב-DB

        public CustomerController(IService<CustomerDto> service /*, IFileUploadService fileUploadService*/)
        {
            _service = service;
            // _fileUploadService = fileUploadService; // כבר לא נחוץ
        }

        // שליפת כל הלקוחות - אסינכרונית
        [HttpGet]
        [Authorize(Roles = "ADMIN, WORKER")]
        public async Task<ActionResult<List<CustomerDto>>> Get()
        {
            try
            {
                var customers = await _service.GetAllAsync();
                return Ok(customers);
            }
            catch (Exception ex)
            {
                // ניתן לרשום את השגיאה ללוג כאן
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // שליפת לקוח לפי מזהה (int id) - אסינכרונית
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<CustomerDto>> Get(int id)
        {
            try
            {
                var customer = await _service.GetByIdAsync(id);
                if (customer == null)
                    return NotFound($"Customer with id {id} not found.");

                // אם customer.ImagePath מכיל כעת את ה-Base64 String, זה יחזור ישירות ללקוח.
                return Ok(customer);
            }
            catch (Exception ex)
            {
                // ניתן לרשום את השגיאה ללוג כאן
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // הוספת לקוח חדש (כולל אפשרות להעלות תמונה) - אסינכרונית
        // יש להגדיר את המתודה כ-[HttpPost] ולקבל [FromForm] CustomerDto
        // כי ה-Service כבר מטפל בהמרת ה-IFormFile ל-byte[]
        [HttpPost]
        [Authorize(Roles = "ADMIN")]
        public async Task<ActionResult<CustomerDto>> Post([FromForm] CustomerDto customerDto)
        {
            if (customerDto == null)
                return BadRequest("Invalid customer data.");

            try
            {
                var createdCustomer = await _service.AddItemAsync(customerDto);
                // הקודם התייחס ל-CustomerCreateRequest שכבר לא בשימוש ישיר.
                // כעת ה-Service מחזיר CustomerDto עם ImagePath כ-Base64 String.
                return CreatedAtAction(nameof(Get), new { id = createdCustomer.Id }, createdCustomer);
            }
            catch (Exception ex)
            {
                // ניתן לרשום את השגיאה ללוג כאן
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        // עדכון לקוח קיים - אסינכרוני
        [HttpPut("{id}")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> Put(int id, [FromForm] CustomerDto customerDto)
        {
            if (id != customerDto.Id)
                return BadRequest("ID mismatch.");

            try
            {
                await _service.UpdateItemAsync(id, customerDto);
                return NoContent(); // 204 No Content - עדכון מוצלח ללא תוכן להחזיר
            }
            catch (KeyNotFoundException ex) // אם הלקוח לא נמצא
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                // ניתן לרשום את השגיאה ללוג כאן
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // מחיקת לקוח - אסינכרוני
        [HttpDelete("{id}")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _service.DeleteItemAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                // ניתן לרשום את השגיאה ללוג כאן
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

       
    }
}