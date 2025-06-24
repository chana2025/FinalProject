using Common.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Interfaces;
using System.Security.Claims;
using System.Threading.Tasks;
using System.IO;
using System;
using Repository.Entities;
using Service.Services;
using Microsoft.AspNetCore.Http;

namespace MyProject.Controllers
{
    //אחראי לקבל בקשות,עיבוד והחזרת תגובה ללקוח-מפנה לניתוב הנכון
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly IService<CustomerDto> _service;
        private readonly IFileUploadService _fileUploadService;
        public CustomerController(IService<CustomerDto> service, IFileUploadService fileUploadService)
        {
            _service = service;
            _fileUploadService = fileUploadService;
        }

        // שליפת כל הלקוחות
        [HttpGet]
        [Authorize(Roles = "ADMIN, WORKER")]
        public ActionResult<List<CustomerDto>> Get()
        {
            try
            {
                var customers = _service.GetAll();
                return Ok(customers);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // שליפת לקוח לפי מזהה (string id)
        [HttpGet("{id}")]
        [Authorize]
        public ActionResult<CustomerDto> Get(int id)
        {
            try
            {
                var customer = _service.GetById(id);
                if (customer == null)
                    return NotFound($"Customer with id {id} not found.");

                return Ok(customer);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        //// הוספת לקוח חדש (כולל אפשרות להעלות תמונה)
        ///נחבר לsignup ב react
        //[HttpPost]
        //[Authorize(Roles = "ADMIN")]    
        //public async Task<ActionResult<CustomerDto>> Post([FromForm] CustomerCreateRequest request)
        //{
        //    if (request == null)
        //        return BadRequest("Invalid request.");

        //    string imagePath = null;
        //    if (request.Image != null && request.Image.Length > 0)
        //    {
        //       //  imagePath = await _fileUploadService.UploadImageAsync(formFile, "dietTypes");

        //    }

        //    CustomerCreateRequest request1 = request;
        //    var customerDto = new CustomerDto
        //    {
        //        Id = request.Id,
        //        FullName=request.FullName,
        //        Email = request.Email,
        //        Phone = request.Phone,
        //        Height = request.Height,
        //        Weight = request.Weight,
        //        Role = request.Role,
        //        //DietType = new DietType(),
        //        //ImagePath = imagePath
        //    };

        //    var createdCustomer = _service.AddItem(customerDto);
        //    return CreatedAtAction(nameof(Get), new { id = createdCustomer.Id }, createdCustomer);
        //}

        // עדכון לקוח קיים
        [HttpPut("{id}")]
        [Authorize(Roles = "ADMIN")]
        public IActionResult Put(int id, [FromForm] CustomerDto customerDto)
        {
            if (id != customerDto.Id)
                return BadRequest("ID mismatch.");

            try
            {
                _service.UpdateItem(id, customerDto);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // מחיקת לקוח
        [HttpDelete("{id}")]
        [Authorize(Roles = "ADMIN")]
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

        [HttpGet("image/{id}")]
        public IActionResult GetCustomerImage(int id)
        {
            var customer = _service.GetById(id);
            if (customer == null || customer.ImagePath == null)
                return NotFound();

            string mimeType = "image/jpeg"; // ברירת מחדל
            if (!string.IsNullOrEmpty(customer.ImageUrl))
            {
                if (customer.ImageUrl.EndsWith(".png", StringComparison.OrdinalIgnoreCase))
                    mimeType = "image/png";
                else if (customer.ImageUrl.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase) ||
                         customer.ImageUrl.EndsWith(".jpeg", StringComparison.OrdinalIgnoreCase))
                    mimeType = "image/jpeg";
            }

            return File(customer.ImagePath, mimeType);
        }



        // פונקציה להעלאת קובץ תמונה
        //private async Task<string> UploadImage(IFormFile file)
        //{
        //    var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images");
        //    if (!Directory.Exists(uploadsFolder))
        //    {
        //        Directory.CreateDirectory(uploadsFolder);
        //    }

        //    var uniqueFileName = $"{Guid.NewGuid()}_{file.FileName}";
        //    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

        //    using (var stream = new FileStream(filePath, FileMode.Create))
        //    {
        //        await file.CopyToAsync(stream);
        //    }

        //    return $"/Images/{uniqueFileName}";
        //}

        //// פונקציה להמיר מזהים לדיאט
        //private List<DietType> GetDietTypeFromFoodIds(List<int> foodIds)
        //{
        //    // חיפוש בדיאטות המתאימות לפי כל מזהי האוכל
        //    var dietTypes = _service.DietTypes.Where(d => foodIds.Contains(d.Id)).ToList();
        //    return dietTypes;
        //}


    }
}