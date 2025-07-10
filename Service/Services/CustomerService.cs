//using AutoMapper;
//using Common.Dto;
//using Repository.Entities;
//using Repository.Interfaces;
//using Service.Interfaces;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;

//namespace Service
//{
//    public class CustomerService : IService<CustomerDto>
//    {
//        private readonly IRepository<Customer> _repository;
//        private readonly IRepository<Product> _productRepository; // הוספתי
//        private readonly IMapper _mapper;

//        public CustomerService(IRepository<Customer> repository, IRepository<Product> productRepository, IMapper mapper)
//        {
//            _repository = repository;
//            _productRepository = productRepository; // הוספתי
//            _mapper = mapper;
//        }

//        public async Task<CustomerDto> GetByIdAsync(int id)
//        {
//            var customer = await _repository.GetByIdAsync(id);
//            return _mapper.Map<CustomerDto>(customer);
//        }

//        public async Task<List<CustomerDto>> GetAllAsync()
//        {
//            var customers = await _repository.GetAllAsync();
//            return _mapper.Map<List<CustomerDto>>(customers);
//        }

//        public async Task<CustomerDto> AddItemAsync(CustomerDto item)
//        {
//            var customer = _mapper.Map<Customer>(item);

//            customer.FoodPreferences = new List<CustomerFoodPreference>();

//            var validProductIds = (await _productRepository.GetAllAsync()).Select(p => p.ProductId).ToHashSet();

//            if (item.LikedProductIds != null)
//            {
//                foreach (var likedId in item.LikedProductIds)
//                {
//                    if (validProductIds.Contains(likedId))
//                    {
//                        customer.FoodPreferences.Add(new CustomerFoodPreference
//                        {
//                            ProductId = likedId,
//                            IsLiked = true
//                        });
//                    }
//                }
//            }

//            if (item.DislikedProductIds != null)
//            {
//                foreach (var dislikedId in item.DislikedProductIds)
//                {
//                    if (validProductIds.Contains(dislikedId))
//                    {
//                        customer.FoodPreferences.Add(new CustomerFoodPreference
//                        {
//                            ProductId = dislikedId,
//                            IsLiked = false
//                        });
//                    }
//                }
//            }

//            var added = await _repository.AddItemAsync(customer);
//            return _mapper.Map<CustomerDto>(added);
//        }

//        public async Task UpdateItemAsync(int id, CustomerDto item)
//        {
//            var customer = _mapper.Map<Customer>(item);

//            customer.FoodPreferences = new List<CustomerFoodPreference>();

//            var validProductIds = (await _productRepository.GetAllAsync()).Select(p => p.ProductId).ToHashSet();

//            if (item.LikedProductIds != null)
//            {
//                foreach (var likedId in item.LikedProductIds)
//                {
//                    if (validProductIds.Contains(likedId))
//                    {
//                        customer.FoodPreferences.Add(new CustomerFoodPreference
//                        {
//                            ProductId = likedId,
//                            IsLiked = true
//                        });
//                    }
//                }
//            }

//            if (item.DislikedProductIds != null)
//            {
//                foreach (var dislikedId in item.DislikedProductIds)
//                {
//                    if (validProductIds.Contains(dislikedId))
//                    {
//                        customer.FoodPreferences.Add(new CustomerFoodPreference
//                        {
//                            ProductId = dislikedId,
//                            IsLiked = false
//                        });
//                    }
//                }
//            }

//            await _repository.UpdateItemAsync(id, customer);
//        }

//        public async Task DeleteItemAsync(int id)
//        {
//            await _repository.DeleteItemAsync(id);
//        }
//    }
//}
using AutoMapper;
using Common.Dto;
using Repository.Entities;
using Repository.Interfaces;
using Service.Interfaces;
using System;
using System.Collections.Generic;
using System.IO; // נוסף עבור MemoryStream
using System.Linq;
using System.Threading.Tasks;

namespace Service
{
    public class CustomerService : IService<CustomerDto>
    {
        private readonly IRepository<Customer> _repository;
        private readonly IRepository<Product> _productRepository;
        private readonly IMapper _mapper;

        public CustomerService(IRepository<Customer> repository, IRepository<Product> productRepository, IMapper mapper)
        {
            _repository = repository;
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task<CustomerDto> GetByIdAsync(int id)
        {
            var customer = await _repository.GetByIdAsync(id);
            var customerDto = _mapper.Map<CustomerDto>(customer);

            // השינוי העיקרי: המרת byte[] ל-Base64 string לפני שליחה ל-Frontend
            // השדה ImagePath ב-CustomerDto ישמש כעת לאחסון ה-Base64.
            if (customer != null && customer.ImagePath != null && customer.ImagePath.Length > 0)
            {
                customerDto.ImagePath = Convert.ToBase64String(customer.ImagePath);
                // הערה: סוג התמונה (image/jpeg, image/png) אינו נשמר כרגע ב-DB.
                // אם תרצה שהפרונטאנד יציג תמיד את התמונה עם Content-Type מדויק,
                // כדאי לשמור גם את סוג התמונה ב-DB (לדוגמה, בשדה חדש ב-Customer).
                // כרגע נניח JPEG או שתטפל בזה בפרונטאנד עם איתור הסוג.
            }
            return customerDto;
        }

        public async Task<List<CustomerDto>> GetAllAsync()
        {
            var customers = await _repository.GetAllAsync();
            var customerDtos = _mapper.Map<List<CustomerDto>>(customers);

            foreach (var customerDto in customerDtos)
            {
                var customer = customers.FirstOrDefault(c => c.Id == customerDto.Id);
                if (customer != null && customer.ImagePath != null && customer.ImagePath.Length > 0)
                {
                    // השינוי העיקרי: המרת byte[] ל-Base64 string עבור כל לקוח ברשימה
                    customerDto.ImagePath = Convert.ToBase64String(customer.ImagePath);
                }
            }
            return customerDtos;
        }

        public async Task<CustomerDto> AddItemAsync(CustomerDto item)
        {
            var customer = _mapper.Map<Customer>(item);

            // השינוי העיקרי: טיפול בקובץ התמונה שהועלה (FileImage)
            // המרת IFormFile ל-byte[] ושמירה ב-ImagePath של הישות.
            if (item.FileImage != null && item.FileImage.Length > 0)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await item.FileImage.CopyToAsync(memoryStream);
                    customer.ImagePath = memoryStream.ToArray(); // שמירת התמונה כ-byte[]
                }
                // ניתן גם לשמור את ה-ContentType אם תרצה לדעת את סוג התמונה
                // customer.ImageContentType = item.FileImage.ContentType; // יש להוסיף שדה כזה ל-Customer entity
            }

            customer.FoodPreferences = new List<CustomerFoodPreference>();
            var validProductIds = (await _productRepository.GetAllAsync()).Select(p => p.ProductId).ToHashSet();

            if (item.LikedProductIds != null)
            {
                foreach (var likedId in item.LikedProductIds)
                {
                    if (validProductIds.Contains(likedId))
                    {
                        customer.FoodPreferences.Add(new CustomerFoodPreference { ProductId = likedId, IsLiked = true });
                    }
                }
            }

            if (item.DislikedProductIds != null)
            {
                foreach (var dislikedId in item.DislikedProductIds)
                {
                    if (validProductIds.Contains(dislikedId))
                    {
                        customer.FoodPreferences.Add(new CustomerFoodPreference { ProductId = dislikedId, IsLiked = false });
                    }
                }
            }

            var added = await _repository.AddItemAsync(customer);
            var addedDto = _mapper.Map<CustomerDto>(added);

            // השינוי העיקרי: המרת byte[] ל-Base64 string לפני החזרה לפרונטאנד, גם לאחר הוספה.
            if (added != null && added.ImagePath != null && added.ImagePath.Length > 0)
            {
                addedDto.ImagePath = Convert.ToBase64String(added.ImagePath);
            }
            return addedDto;
        }

        public async Task UpdateItemAsync(int id, CustomerDto item)
        {
            var existingCustomer = await _repository.GetByIdAsync(id);
            if (existingCustomer == null)
            {
                throw new KeyNotFoundException($"Customer with ID {id} not found.");
            }

            // עדכון שדות קיימים מה-DTO אל הישות הקיימת
            // אנו משתמשים ב-Map של AutoMapper כדי לעדכן את השדות הקיימים
            // אך נטפל ב-ImagePath ו-FoodPreferences ידנית כדי לשלוט בלוגיקה.
            _mapper.Map(item, existingCustomer);

            // השינוי העיקרי: טיפול בקובץ התמונה שהועלה בעדכון.
            // אם הועלה קובץ חדש (FileImage אינו null ויש לו תוכן), הוא ידרוס את התמונה הקיימת ב-DB.
            if (item.FileImage != null && item.FileImage.Length > 0)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await item.FileImage.CopyToAsync(memoryStream);
                    existingCustomer.ImagePath = memoryStream.ToArray(); // שמירת התמונה כ-byte[]
                }
            }
            // אם item.FileImage הוא null (כלומר, לא נשלח קובץ חדש),
            // ה-existingCustomer.ImagePath יישאר ללא שינוי, וישמור את התמונה הקיימת ב-DB.
            // אם תרצה אפשרות למחוק תמונה באופן מפורש, נצטרך להוסיף לוגיקה נוספת (לדוגמה, שדה בוליאני ב-DTO כמו `DeleteImage`).

            // טיפול ב-FoodPreferences:
            // חשוב לטעון את FoodPreferences לפני המחיקה וההוספה, כדי למנוע שגיאות Tracking של EF Core.
            // וודא שמתודת GetByIdAsync בריפוזיטורי שלך (CustomerRepository) כוללת את טעינת ה-FoodPreferences
            // (באמצעות `.Include(c => c.FoodPreferences)`).
            // אם לא, תקבל שגיאת NullReferenceException.
            if (existingCustomer.FoodPreferences != null)
            {
                existingCustomer.FoodPreferences.Clear(); // מנקה את ההעדפות הקיימות
            }
            else
            {
                existingCustomer.FoodPreferences = new List<CustomerFoodPreference>();
            }


            var validProductIds = (await _productRepository.GetAllAsync()).Select(p => p.ProductId).ToHashSet();

            if (item.LikedProductIds != null)
            {
                foreach (var likedId in item.LikedProductIds)
                {
                    if (validProductIds.Contains(likedId))
                    {
                        existingCustomer.FoodPreferences.Add(new CustomerFoodPreference { ProductId = likedId, IsLiked = true });
                    }
                }
            }

            if (item.DislikedProductIds != null)
            {
                foreach (var dislikedId in item.DislikedProductIds)
                {
                    if (validProductIds.Contains(dislikedId))
                    {
                        existingCustomer.FoodPreferences.Add(new CustomerFoodPreference { ProductId = dislikedId, IsLiked = false });
                    }
                }
            }

            await _repository.UpdateItemAsync(id, existingCustomer);
        }

        public async Task DeleteItemAsync(int id)
        {
            await _repository.DeleteItemAsync(id);
        }
    }
}