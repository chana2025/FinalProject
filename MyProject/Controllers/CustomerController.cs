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
using System.Collections.Generic;

namespace MyProject.Controllers
{
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

                return Ok(customer);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // הוספת לקוח חדש (כולל אפשרות להעלות תמונה) - אסינכרונית
        //[HttpPost]
        //[Authorize(Roles = "ADMIN")]
        //public async Task<ActionResult<CustomerDto>> Post([FromForm] CustomerCreateRequest request)
        //{
        //    if (request == null)
        //        return BadRequest("Invalid request.");

        //    string imagePath = null;
        //    if (request.Image != null && request.Image.Length > 0)
        //    {
        //       imagePath = await _fileUploadService.UploadImageAsync(request.Image, "dietTypes");
        //    }

        //    var customerDto = new CustomerDto
        //    {
        //        Id = request.Id,
        //        FullName = request.FullName,
        //        Email = request.Email,
        //        Phone = request.Phone,
        //        Height = request.Height,
        //        Weight = request.Weight,
        //        Role = request.Role,
        //        ImagePath = imagePath
        //    };

        //    var createdCustomer = await _service.AddItemAsync(customerDto);
        //    return CreatedAtAction(nameof(Get), new { id = createdCustomer.Id }, createdCustomer);
        //}

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
                return NoContent();
            }
            catch (Exception ex)
            {
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
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("image/{id}")]
        public async Task<IActionResult> GetCustomerImage(int id)
        {
            var customer = await _service.GetByIdAsync(id);
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
    }
}
