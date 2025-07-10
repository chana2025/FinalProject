using Common.Dto;
using Microsoft.AspNetCore.Mvc;
using Service.Interfaces;
using Service.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DietTypeController : ControllerBase
    {
        private readonly IService<DietDto> _service;
        private readonly IFileUploadService _fileUploadService;
        private readonly GreedyAlg _greedyAlg;

        public DietTypeController(IFileUploadService fileUploadService, IService<DietDto> service, GreedyAlg greedyAlg)
        {
            _fileUploadService = fileUploadService;
            _service = service;
            _greedyAlg = greedyAlg;
        }

        // GET: api/DietType
        [HttpGet]
        public async Task<ActionResult<List<DietDto>>> Get()
        {
            try
            {
                var diets = await _service.GetAllAsync();
                return Ok(diets);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // GET: api/DietType/5
        // מחזיר את הדיאטה עם הסברים, המלצות ודוגמאות לפי שם הדיאטה
        [HttpGet("{id}")]
        public async Task<ActionResult<DietDto>> Get(int id)
        {
            try
            {
                var diet = await _service.GetByIdAsync(id);
                if (diet == null)
                    return NotFound($"Diet with id {id} not found.");

                return Ok(diet);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        // POST: api/DietType
        [HttpPost]
        public async Task<IActionResult> Post([FromForm] DietDto diet)
        {
            if (diet == null)
                return BadRequest("Invalid Diet data.");

            try
            {
                if (diet.fileImage != null && diet.fileImage.Length > 0)
                {
                    var imagePath = await _fileUploadService.UploadImageAsync(diet.fileImage, "dietTypes");
                    diet.ImageUrl = imagePath;
                }

                await _service.AddItemAsync(diet);
                return Ok("Diet added successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // PUT: api/DietType/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromForm] DietDto dietDto)
        {
            if (id != dietDto.DietId)
                return BadRequest("ID mismatch.");

            try
            {
                await _service.UpdateItemAsync(id, dietDto);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // DELETE: api/DietType/5
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

        // פונקציה חדשה - התאמה חמדנית של דיאטה ללקוח
        [HttpGet("match-diet/{customerId}")]
        public IActionResult MatchDietToCustomer(int customerId)
        {
            try
            {
                var matchedDiet = _greedyAlg.MatchBestDietsForCustomer(customerId);
                if (matchedDiet == null)
                    return NotFound("No suitable diet found for this customer.");

                return Ok(matchedDiet);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // פונקציה פנימית להחזרת הסבר ודוגמאות לפי שם דיאטה
        private object GetDietExplanation(string dietName)
        {
            switch (dietName.ToLower())
            {
                case "keto":
                    return new
                    {
                        Overview = "תפריט קטוגני (Keto) מתבסס על מזון עתיר שומן ודל פחמימות, כדי להכניס את הגוף למצב של קטוזיס.",
                        Recommendations = new[] {
                            "העדיפי אבוקדו, שמן קוקוס, אגוזים, ביצים, גבינות שמנות, עוף ודגים.",
                            "הימנעי מלחמים, סוכר, פסטה, אורז ותפוחי אדמה."
                        },
                        SampleMenu = new[] {
                            "בוקר: חביתה עם אבוקדו וגבינה צהובה",
                            "צהריים: עוף בגריל עם סלט ירוק עשיר בשמן זית",
                            "ערב: סלמון בתנור עם ברוקולי מאודה"
                        }
                    };

                case "points diet":
                    return new
                    {
                        Overview = "בדיאטת הנקודות אין איסורים – רק ספירת נקודות על בסיס כמות ואיכות.",
                        Recommendations = new[] {
                            "הכירי את מספר הנקודות המותר ביום לפי משקל/גובה.",
                            "בחרי מאכלים בעלי ערך תזונתי גבוה וערך נקודות נמוך."
                        },
                        SampleMenu = new[] {
                            "בוקר: יוגורט עם גרנולה (2 נקודות)",
                            "ביניים: תפוח (1 נקודה)",
                            "צהריים: חזה עוף עם אורז מלא וירקות (5 נקודות)"
                        }
                    };

                case "high carb":
                    return new
                    {
                        Overview = "דיאטה עשירה בפחמימות – מתאימה לספורטאים או אנשים עם חילוף חומרים מהיר.",
                        Recommendations = new[] {
                            "שלבי פחמימות איכותיות: קוואקר, בטטה, פירות, אורז מלא.",
                            "שמרי על איזון עם חלבון ודאגי לשתייה מרובה."
                        },
                        SampleMenu = new[] {
                            "בוקר: דייסת שיבולת שועל עם בננה",
                            "צהריים: פסטה מחיטה מלאה עם רוטב עגבניות",
                            "ערב: סנדוויץ' מלחם מלא עם טונה"
                        }
                    };

                default:
                    return new
                    {
                        Overview = "אין מידע נוסף על הדיאטה הזו כרגע.",
                        Recommendations = new string[] { },
                        SampleMenu = new string[] { }
                    };
            }
        }
    }
}
