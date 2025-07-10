//using AutoMapper;
//using Common.Dto;
//using Repository.Entities;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Runtime;
//using System.Text;
//using System.Threading.Tasks;
//namespace Service
//{
//    public class MyMapper : Profile
//    {
//        string imagesFolderPath = Path.Combine(Environment.CurrentDirectory, "Images");

//        public MyMapper()
//        {
//            CreateMap<Customer, CustomerDto>()
//// קודם: File.ReadAllBytes
//// עכשיו: Convert.ToBase64String(File.ReadAllBytes(...))
//.ForMember(dest => dest.ImagePath, opt => opt.MapFrom(src =>
//    File.Exists(Path.Combine(imagesFolderPath, src.ImageUrl))
//        ? Convert.ToBase64String(File.ReadAllBytes(Path.Combine(imagesFolderPath, src.ImageUrl)))
//        : null));

//            CreateMap<CustomerDto, Customer>()
//                            .ForMember(dest => dest.ImagePath, opt =>
//                                opt.MapFrom(src =>
//                                    string.IsNullOrEmpty(src.ImagePath)
//                                        ? null
//                                        : Convert.FromBase64String(src.ImagePath)));

//            CreateMap<Customer, CustomerDto>()
//                .ForMember(dest => dest.ImagePath, opt =>
//                    opt.MapFrom(src =>
//                        src.ImagePath != null
//                            ? Convert.ToBase64String(src.ImagePath)
//                            : null));

//            CreateMap<DietType, DietDto>()
//                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.ImageUrl))
//                .ReverseMap()
//                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.ImageUrl));

//            CreateMap<WeeklyTracking, WeeklyTrackingDto>().ReverseMap();
//            CreateMap<Product, ProductDto>().ReverseMap();
//            CreateMap<CustomerFoodPreference, FoodPreferencesDto>().ReverseMap();
//        }
//    }

//}

using AutoMapper;
using Common.Dto;
using Repository.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO; // ודא שקיים לצורך Convert.ToBase64String

namespace Service
{
    public class MyMapper : Profile
    {
        public MyMapper()
        {
            // מיפוי מ-Customer (Entity) ל-CustomerDto (Dto)
            CreateMap<Customer, CustomerDto>()
                // כאשר ממפים מ-Customer (שדה ImagePath הוא byte[]) ל-CustomerDto (שדה ImagePath הוא string),
                // אנו רוצים להתעלם מהמיפוי האוטומטי כאן. 
                // הלוגיקה של המרת byte[] ל-Base64 String מתבצעת באופן מפורש ב-CustomerService.
                //.ForMember(dest => dest.ImagePath, opt => opt.Ignore())
                // גם את FileImage (IFormFile) אנו מתעלמים, כי הוא רלוונטי רק לכיוון Dto -> Entity
                .ForMember(dest => dest.FileImage, opt => opt.Ignore())
                // מיפוי של FoodPreferences לרשימות ה-IDs
                .ForMember(dest => dest.LikedProductIds, opt =>
                    opt.MapFrom(src => src.FoodPreferences.Where(fp => fp.IsLiked).Select(fp => fp.ProductId).ToList()))
                .ForMember(dest => dest.DislikedProductIds, opt =>
                    opt.MapFrom(src => src.FoodPreferences.Where(fp => !fp.IsLiked).Select(fp => fp.ProductId).ToList()));

            // מיפוי מ-CustomerDto (Dto) ל-Customer (Entity)
            CreateMap<CustomerDto, Customer>()
                // כאשר ממפים מ-CustomerDto (ששדה FileImage הוא IFormFile) ל-Customer (שדה ImagePath הוא byte[]),
                // אנו רוצים להתעלם מהמיפוי האוטומטי כאן. 
                // הלוגיקה של המרת IFormFile ל-byte[] מתבצעת באופן מפורש ב-CustomerService.
                .ForMember(dest => dest.ImagePath, opt => opt.Ignore())
                // FoodPreferences מטופלים ידנית ב-Service, ולכן מתעלמים מהם במיפוי
                .ForMember(dest => dest.FoodPreferences, opt => opt.Ignore())
                // DietType הוא אובייקט מקושר, אין למפות אותו ישירות מ-DTO אלא דרך DietId
                .ForMember(dest => dest.DietType, opt => opt.Ignore());

            // מיפוי מ-SignUpDto (Dto) ל-Customer (Entity) עבור הרשמה
            CreateMap<SignUpDto, Customer>()
                // גם כאן, ImagePath (byte[]) יטופל ידנית ב-Service מה-FileImage (IFormFile)
                .ForMember(dest => dest.ImagePath, opt => opt.Ignore())
                // FoodPreferences מטופלים ידנית בלוגיקה ב-Service
                .ForMember(dest => dest.FoodPreferences, opt => opt.Ignore())
                // DietType הוא אובייקט מקושר, אין למפות אותו ישירות מ-DTO אלא דרך DietId
                .ForMember(dest => dest.DietType, opt => opt.Ignore());

            // מיפויים נוספים (נשארים כפי שהם, בהנחה שהם תקינים למודל שלך)
            // מיפוי DietType: אם ImageUrl ב-DietType הוא נתיב פיזי, אולי תצטרך לטפל בו ב-Service
            // או לוודא שה-path הוא URL אם הוא מכוון ל-Frontend.
            CreateMap<DietType, DietDto>().ReverseMap();
            CreateMap<WeeklyTracking, WeeklyTrackingDto>().ReverseMap();
            CreateMap<Product, ProductDto>().ReverseMap();
            CreateMap<CustomerFoodPreference, FoodPreferencesDto>().ReverseMap();
        }
    }
}
