//using Common.Dto;
//using Service.Interfaces;
//using System.Net.Http;
//using System.Text.Json;

//namespace Service
//{
//    public class ProductApiService : IProductApiService
//    {
//        private readonly HttpClient _httpClient;

//        public ProductApiService(HttpClient httpClient)
//        {
//            _httpClient = httpClient;
//        }

//        public async Task<List<ProductDto>> GetAllProductsAsync()
//        {
//            return await GetAllProductsExtendedAsync(3000); // יעד של 3000 מוצרים
//        }

//        private async Task<List<ProductDto>> GetAllProductsExtendedAsync(int targetCount)
//        {
//            var all = new List<ProductDto>();
//            int page = 1, pageSize = 100;

//            while (all.Count < targetCount)
//            {
//                string url = $"https://world.openfoodfacts.org/cgi/search.pl?search_terms=&search_simple=1&action=process&json=1&page={page}&page_size={pageSize}";
//                var json = await _httpClient.GetStringAsync(url);

//                var result = JsonSerializer.Deserialize<OpenFoodResult>(json, new JsonSerializerOptions
//                {
//                    PropertyNameCaseInsensitive = true
//                });

//                var items = result?.products?
//                    .Where(p => !string.IsNullOrEmpty(p.product_name))
//                    .Select(p => new ProductDto
//                    {
//                        Name = p.product_name,
//                        Calories = p.nutriments?.energy_100g,
//                        Protein = p.nutriments?.proteins_100g,
//                        Fat = p.nutriments?.fat_100g,
//                        Carbohydrates = p.nutriments?.carbohydrates_100g,
//                        SourceApi = "OpenFoodFacts"
//                    }).ToList();

//                if (items == null || items.Count == 0)
//                    break;

//                all.AddRange(items);
//                page++;
//            }

//            return all.Take(targetCount).ToList();
//        }

//        public async Task<ProductDto> GetProductByIdAsync(string id)
//        {
//            return null; // אפשר לממש בהמשך
//        }

//        // מחלקות עזר לדסיריאליזציה (אפשר גם להוציא לקובץ נפרד)
//        private class OpenFoodResult
//        {
//            public List<OpenFoodProduct> products { get; set; }
//        }

//        private class OpenFoodProduct
//        {
//            public string product_name { get; set; }
//            public Nutriments nutriments { get; set; }
//        }

//        private class Nutriments
//        {
//            public double? energy_100g { get; set; }
//            public double? proteins_100g { get; set; }
//            public double? fat_100g { get; set; }
//            public double? carbohydrates_100g { get; set; }
//        }
//    }
//}
using Common.Dto;
using Service.Interfaces;
using System.Net.Http;
using System.Text.Json;

namespace Service
{
    /// <summary>
    /// שירות למשיכת מוצרים מממשק OpenFoodFacts
    /// </summary>
    public class ProductApiService : IProductApiService
    {
        private readonly HttpClient _httpClient;

        /// <summary>
        /// בנאי שמקבל HttpClient לצורך קריאות לרשת
        /// </summary>
        public ProductApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        /// <summary>
        /// שיטה ציבורית שמחזירה עד 3000 מוצרים מה-API
        /// </summary>
        public async Task<List<ProductDto>> GetAllProductsAsync()
        {
            return await GetAllProductsExtendedAsync(3000); // יעד של 3000 מוצרים
        }

        /// <summary>
        /// שיטה פרטית שמבצעת את שליפת המוצרים מדפים מרובים, עד שמגיעים לכמות היעד
        /// </summary>
        /// <param name="targetCount">כמות מוצרים רצויה</param>
        private async Task<List<ProductDto>> GetAllProductsExtendedAsync(int targetCount)
        {
            var all = new List<ProductDto>();
            int page = 1, pageSize = 100;

            // ממשיך לטעון עמודים עד שמגיע לכמות המבוקשת
            while (all.Count < targetCount)
            {
                string url = $"https://world.openfoodfacts.org/cgi/search.pl?search_terms=&search_simple=1&action=process&json=1&page={page}&page_size={pageSize}";
                var json = await _httpClient.GetStringAsync(url);

                // המרה של המחרוזת לאובייקט
                var result = JsonSerializer.Deserialize<OpenFoodResult>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                // ממיר את הנתונים לרשימת ProductDto
                var items = result?.products?
                    .Where(p => !string.IsNullOrEmpty(p.product_name))
                    .Select(p => new ProductDto
                    {
                        Name = p.product_name,
                        Calories = p.nutriments?.energy_100g,
                        Protein = p.nutriments?.proteins_100g,
                        Fat = p.nutriments?.fat_100g,
                        Carbohydrates = p.nutriments?.carbohydrates_100g,
                        SourceApi = "OpenFoodFacts"
                    }).ToList();

                // אם לא חזרו נתונים — יוצא מהלולאה
                if (items == null || items.Count == 0)
                    break;

                // מוסיף את הפריטים שנמצאו לרשימה הראשית
                all.AddRange(items);
                page++; // עובר לעמוד הבא
            }

            // מחזיר את כמות המוצרים שצריך בלבד
            return all.Take(targetCount).ToList();
        }

        /// <summary>
        /// שיטה לשליפת מוצר לפי מזהה — טרם מומשה
        /// </summary>
        public async Task<ProductDto> GetProductByIdAsync(string id)
        {
            return null; // אפשר לממש בהמשך
        }

        // === מחלקות עזר לדסיריאליזציה של JSON ===
        private class OpenFoodResult
        {
            public List<OpenFoodProduct> products { get; set; }
        }

        private class OpenFoodProduct
        {
            public string product_name { get; set; }
            public Nutriments nutriments { get; set; }
        }

        private class Nutriments
        {
            public double? energy_100g { get; set; }
            public double? proteins_100g { get; set; }
            public double? fat_100g { get; set; }
            public double? carbohydrates_100g { get; set; }
        }
    }
}

