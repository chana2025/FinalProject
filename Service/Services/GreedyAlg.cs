using Microsoft.EntityFrameworkCore;
using Repository.Entities;
using Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Service.Services
{
    public class GreedyAlg
    {
        private readonly IContext _context;

        public GreedyAlg(IContext context)
        {
            _context = context;
        }

        /// <summary>
        /// מתממשק למסד הנתונים, בודק אילו מוצרים הלקוח אוהב, ומשדך לו את הדיאטות שהכי מתאימות לו
        /// לפי אלגוריתם חמדני בהתאם לרכיב הדומיננטי של כל דיאטה
        /// </summary>
        public List<DietType> MatchBestDietsForCustomer(int customerId)
        {
            // שלב 1: שליפת כל המוצרים שהלקוח סימן כאהובים
            var likedProducts = _context.CustomerFoodPreferences
                .Where(p => p.CustomerId == customerId && p.IsLiked)
                .Include(p => p.Product)
                .Select(p => p.Product)
                .ToList();

            Console.WriteLine($"Customer {customerId} liked products count: {likedProducts.Count}");

            // אם אין מוצרים אהובים, מחזיר את דיאטת ברירת המחדל (ID=6)
            if (!likedProducts.Any())
            {
                Console.WriteLine("No liked products found, returning default diet ID=6");
                return new List<DietType> { _context.DietTypes.FirstOrDefault(d => d.Id == 6) };
            }

            // שלב 2: שליפת כל הדיאטות הקיימות
            var diets = _context.DietTypes.ToList();
            var suitableDiets = new List<(DietType diet, int includedCount)>();
            int maxIncluded = 0;

            // חישוב ערכים מקסימליים של כל רכיב עבור קביעת דומיננטיות
            double maxCal = diets.Max(d => d.Calories ?? 0);
            double maxPro = diets.Max(d => d.Protein ?? 0);
            double maxFat = diets.Max(d => d.Fat ?? 0);
            double maxCarb = diets.Max(d => d.Carbohydrates ?? 0);

            // שלב 3: מעבר על כל דיאטה לבדיקה איזה מוצרים אפשר "להכניס" אליה
            foreach (var diet in diets)
            {
                double dCal = diet.Calories ?? 0;
                double dPro = diet.Protein ?? 0;
                double dFat = diet.Fat ?? 0;
                double dCarb = diet.Carbohydrates ?? 0;

                /// קביעת הרכיב הדומיננטי של הדיאטה (הכי גבוה יחסית לאחרות)
                var dominance = new Dictionary<string, double>
                {
                    { "Calories", maxCal == 0 ? 0 : dCal / maxCal },
                    { "Protein", maxPro == 0 ? 0 : dPro / maxPro },
                    { "Fat", maxFat == 0 ? 0 : dFat / maxFat },
                    { "Carbohydrates", maxCarb == 0 ? 0 : dCarb / maxCarb }
                };

                string dominant = dominance.OrderByDescending(kv => kv.Value).First().Key;

                // קביעת גבולות הדיאטה לכל רכיב
                double limitCal = dCal == 0 ? double.MaxValue : dCal;
                double limitPro = dPro == 0 ? double.MaxValue : dPro;
                double limitFat = dFat == 0 ? double.MaxValue : dFat;
                double limitCarb = dCarb == 0 ? double.MaxValue : dCarb;

                // ממיין את המוצרים לפי הרכיב הדומיננטי של הדיאטה
                IEnumerable<Product> sortedProducts = dominant switch
                {
                    "Protein" => likedProducts.OrderByDescending(p => p.Protein ?? 0),
                    "Fat" => likedProducts.OrderByDescending(p => p.Fat ?? 0),
                    "Carbohydrates" => likedProducts.OrderByDescending(p => p.Carbohydrates ?? 0),
                    _ => likedProducts.OrderBy(p => p.Calories ?? 0)
                };

                // הכנסת מוצרים לדיאטה בצורה חמדנית כל עוד לא חורגים מהמגבלות
                double sumCal = 0, sumPro = 0, sumFat = 0, sumCarb = 0;
                int included = 0;

                foreach (var p in sortedProducts)
                {
                    if (sumCal + (p.Calories ?? 0) <= limitCal &&
                        sumPro + (p.Protein ?? 0) <= limitPro &&
                        sumFat + (p.Fat ?? 0) <= limitFat &&
                        sumCarb + (p.Carbohydrates ?? 0) <= limitCarb)
                    {
                        sumCal += p.Calories ?? 0;
                        sumPro += p.Protein ?? 0;
                        sumFat += p.Fat ?? 0;
                        sumCarb += p.Carbohydrates ?? 0;
                        included++;
                    }
                }

                Console.WriteLine($"Diet ID={diet.Id} - {diet.DietName}: included {included} liked products (dominant: {dominant})");

                if (included > maxIncluded)
                    maxIncluded = included;

                suitableDiets.Add((diet, included));
            }

            // חישוב שיעור ההתאמה
            double matchRate = (double)maxIncluded / likedProducts.Count;
            Console.WriteLine($"Best match includes {maxIncluded} products out of {likedProducts.Count} ({matchRate:P})");

            // אם ההתאמה נמוכה מדי, מחזיר דיאטה ברירת מחדל
            if (matchRate <= 0.5)
            {
                Console.WriteLine("Match too low, returning default diet ID=6");
                return new List<DietType> { _context.DietTypes.FirstOrDefault(d => d.Id == 6) };
            }

            // שלב 4: מחזיר את כל הדיאטות עם מספר ההתאמות הגבוה ביותר
            var bestDiets = suitableDiets
                .Where(d => d.includedCount == maxIncluded)
                .Select(d => d.diet)
                .ToList();

            Console.WriteLine($"Returning {bestDiets.Count} best matching diets");
            return bestDiets;
        }
    }
}
