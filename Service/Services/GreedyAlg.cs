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

        public List<DietType> MatchBestDietsForCustomer(int customerId)
        {
            // 1. טוען את המוצרים שהלקוח אוהב
            var likedProducts = _context.CustomerFoodPreferences
                .Where(p => p.CustomerId == customerId && p.IsLiked)
                .Include(p => p.Product)
                .Select(p => p.Product)
                .ToList();

            Console.WriteLine($"Customer {customerId} liked products count: {likedProducts.Count}");

            if (!likedProducts.Any())
            {
                Console.WriteLine("No liked products found, returning default diet ID=6");
                return new List<DietType> { _context.DietTypes.FirstOrDefault(d => d.Id == 6) };
            }

            // 2. טוען את כל הדיאטות
            var diets = _context.DietTypes.ToList();
            var suitableDiets = new List<(DietType diet, int includedCount)>();
            int maxIncluded = 0;

            foreach (var diet in diets)
            {
                double maxCal = diet.Calories ?? double.MaxValue;
                double maxPro = diet.Protein ?? double.MaxValue;
                double maxFat = diet.Fat ?? double.MaxValue;
                double maxCarb = diet.Carbohydrates ?? double.MaxValue;

                double totalCal = 0, totalPro = 0, totalFat = 0, totalCarb = 0;
                int included = 0;

                foreach (var p in likedProducts.OrderBy(p => p.Calories))
                {
                    if (totalCal + (p.Calories ?? 0) <= maxCal &&
                        totalPro + (p.Protein ?? 0) <= maxPro &&
                        totalFat + (p.Fat ?? 0) <= maxFat &&
                        totalCarb + (p.Carbohydrates ?? 0) <= maxCarb)
                    {
                        totalCal += p.Calories ?? 0;
                        totalPro += p.Protein ?? 0;
                        totalFat += p.Fat ?? 0;
                        totalCarb += p.Carbohydrates ?? 0;
                        included++;
                    }
                }

                Console.WriteLine($"Diet ID={diet.Id} - {diet.DietName}: included {included} liked products");

                if (included > maxIncluded)
                    maxIncluded = included;

                suitableDiets.Add((diet, included));
            }

            double matchRate = (double)maxIncluded / likedProducts.Count;
            Console.WriteLine($"Best match includes {maxIncluded} products out of {likedProducts.Count} ({matchRate:P})");

            if (matchRate <= 0.5)
            {
                Console.WriteLine("Match too low, returning default diet ID=6");
                return new List<DietType> { _context.DietTypes.FirstOrDefault(d => d.Id == 6) };
            }

            // 3. מחזיר את כל הדיאטות עם מספר ההתאמות המקסימלי
            var bestDiets = suitableDiets
                .Where(d => d.includedCount == maxIncluded)
                .Select(d => d.diet)
                .ToList();

            Console.WriteLine($"Returning {bestDiets.Count} best matching diets");
            return bestDiets;
        }
    }
}