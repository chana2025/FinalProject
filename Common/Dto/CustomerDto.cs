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

        // התמונה עצמה (לשימוש ב-API שרוצה להחזיר אותה כ-base64 או File)
        public string ImagePath { get; set; }

        // שם הקובץ או הנתיב (אם נשמרה פיזית בתיקייה, לדוג' /images/user1.jpg)
        public string? ImageUrl { get; set; }

        public List<int> LikedProductIds { get; set; } = new();
        public List<int> DislikedProductIds { get; set; } = new();
        // קובץ שמגיע מהקליינט בהרשמה
        public IFormFile? FileImage { get; set; }
    }

}
