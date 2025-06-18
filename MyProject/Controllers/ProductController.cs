using Common.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Interfaces; // ודא שזה מיובא
using System;
using System.Collections.Generic;

namespace MyProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        // ✅ שינוי: הזרקת IProductService במקום IService<ProductDto>
        private readonly IProductService _service; // שים לב לשינוי כאן

        // ✅ שינוי: הקונסטרקטור צריך לקבל IProductService
        public ProductController(IProductService service)
        {
            _service = service;
        }

        [HttpGet]
        // [Authorize(Roles = "ADMIN,WORKER")] // שחזר את ההערה אם זה עדיין מיועד
        public ActionResult<List<ProductDto>> Get()
        {
            try
            {
                return Ok(_service.GetAll());
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("{id}")]
        //[Authorize] // שחזר את ההערה אם זה עדיין מיועד
        public ActionResult<ProductDto> Get(int id)
        {
            try
            {
                var product = _service.GetById(id);
                if (product == null)
                    return NotFound($"Product with ID {id} not found");

                return Ok(product);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // ✅ הוספה: נקודת קצה חדשה לחיפוש מוצרים לפי שם
        [HttpGet("search")] // לדוגמה: /api/Product/search?name=apple
        // [Authorize] // ייתכן ותרצי הרשאה גם לחיפוש, תלוי אם זה פתוח לכל המשתמשים
        public ActionResult<List<ProductDto>> SearchProducts([FromQuery] string name)
        {
            try
            {
                // ודא ששם החיפוש סופק
                if (string.IsNullOrWhiteSpace(name))
                {
                    // אפשר להחזיר BadRequest או פשוט את כל המוצרים אם שם החיפוש ריק
                    // במקרה זה, נחזיר Bad Request אם אין שם חיפוש
                    return BadRequest("שם המוצר לחיפוש אינו יכול להיות ריק.");
                }

                var products = _service.GetByName(name);
                if (products == null || products.Count == 0)
                {
                    return NotFound($"לא נמצאו מוצרים המכילים את השם '{name}'.");
                }
                return Ok(products);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        [Authorize(Roles = "ADMIN")]
        public ActionResult<ProductDto> Post([FromForm] ProductDto product)
        {
            try
            {
                var created = _service.AddItem(product);
                // ודא ש-ProductId קיים ב-created. אם לא, יכול להיות בעיה במיפוי או בשמירה
                return CreatedAtAction(nameof(Get), new { id = created.ProductId }, created);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "ADMIN")]
        public IActionResult Put(int id, [FromForm] ProductDto product)
        {
            if (id != product.ProductId) // ודא שזה ProductId ולא CustomerId
                return BadRequest("ID mismatch");

            try
            {
                _service.UpdateItem(id, product);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

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
                return StatusCode(500, ex.Message);
            }
        }
    }
}