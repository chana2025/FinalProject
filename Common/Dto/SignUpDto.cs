using Microsoft.AspNetCore.Http;
using Repository.Entities;
using System.Collections.Generic;

namespace Common.Dto
{
    public class SignUpDto
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Password { get; set; }
        public double? Height { get; set; }
        public double? Weight { get; set; }
        public eRole Role { get; set; }
        public eGender Gender { get; set; }
        public List<int> LikedProductIds { get; set; } = new();
        public List<int> DislikedProductIds { get; set; } = new();

        public IFormFile? FileImage { get; set; }
    }
}
