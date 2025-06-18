using Microsoft.AspNetCore.Http;
using Repository.Entities;

namespace Common.Dto
{
    public class CustomerDto
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Phone { get; set; }
        public eRole Role { get; set; }
        public eGender Gender { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public double? Height { get; set; }
        public double? Weight { get; set; }
        public byte[]? ImagePath { get; set; } // שמירת תוכן התמונה
        public IFormFile? FileImage { get; set; } // מגיע מהלקוח (מה־Form)
    }
}
