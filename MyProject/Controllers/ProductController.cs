using Common.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using ClosedXML.Excel;

namespace MyProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _service;
        private readonly IProductApiService _apiService;

        public ProductController(IProductService service, IProductApiService apiService)
        {
            _service = service;
            _apiService = apiService;
        }

        [HttpGet]
        public async Task<ActionResult<List<ProductDto>>> Get()
        {
            try
            {
                var products = await _service.GetAllAsync();
                return Ok(products);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDto>> Get(int id)
        {
            try
            {
                var product = await _service.GetByIdAsync(id);
                if (product == null)
                    return NotFound($"Product with ID {id} not found");

                return Ok(product);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("search")]
        public async Task<ActionResult<List<ProductDto>>> SearchProducts([FromQuery] string name)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(name))
                    return BadRequest("שם המוצר לחיפוש אינו יכול להיות ריק.");

                var products = await _service.GetByNameAsync(name);
                if (products == null || products.Count == 0)
                    return NotFound($"לא נמצאו מוצרים המכילים את השם '{name}'.");

                return Ok(products);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        [Authorize(Roles = "ADMIN")]
        public async Task<ActionResult<ProductDto>> Post([FromForm] ProductDto product)
        {
            try
            {
                var created = await _service.AddItemAsync(product);
                return CreatedAtAction(nameof(Get), new { id = created.ProductId }, created);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> Put(int id, [FromForm] ProductDto product)
        {
            if (id != product.ProductId)
                return BadRequest("ID mismatch");

            try
            {
                await _service.UpdateItemAsync(id, product);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

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
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("import-from-api")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> ImportFromApi()
        {
            try
            {
                var productsFromApi = await _apiService.GetAllProductsAsync();
                int added = await _service.SaveProductsFromApiAsync(productsFromApi);
                return Ok($"{added} products were imported successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error importing products: {ex.Message}");
            }
        }

        [HttpGet("download-excel")]
        public async Task<IActionResult> DownloadExcel()
        {
            try
            {
                var products = await _service.GetAllAsync();

                using var workbook = new XLWorkbook();
                var worksheet = workbook.Worksheets.Add("Products");
                worksheet.Cell(1, 1).InsertTable(products);

                using var stream = new MemoryStream();
                workbook.SaveAs(stream);
                stream.Position = 0;

                string fileName = $"products_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";

                return File(stream.ToArray(),
                            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                            fileName);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "שגיאה ביצירת הקובץ: " + ex.Message);
            }
        }
    }
}
