using Common.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Service.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MyProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IService<CustomerDto> _service;
        private readonly IConfiguration _config;

        public LoginController(IService<CustomerDto> service, IConfiguration config)
        {
            _service = service;
            _config = config;
        }

        // POST api/login/login (כניסה למערכת)
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromForm] UserLogin userLogin)
        {
            var user = await AuthenticateAsync(userLogin);
            if (user == null)
                return Unauthorized("User not found.");

            var token = GenerateToken(user);

            // מחזיר גם את הטוקן וגם את מזהה המשתמש
            return Ok(new { token, id = user.Id });
        }

        // PUT api/login/{id} (עדכון לקוח קיים)
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromForm] CustomerDto customer)
        {
            var existingCustomer = await _service.GetByIdAsync(id);
            if (existingCustomer == null)
                return NotFound("Customer not found.");

            customer.Id = id;
            await _service.UpdateItemAsync(id, customer);
            return Ok("Customer updated successfully.");
        }

        // פונקציית אימות אסינכרונית
        private async Task<CustomerDto> AuthenticateAsync(UserLogin userLogin)
        {
            var email = userLogin.Email.Trim().ToLower();
            var password = userLogin.Password.Trim();

            var allUsers = await _service.GetAllAsync();
            return allUsers.FirstOrDefault(u => u.Email.Trim().ToLower() == email &&
                                                u.Password.Trim() == password);
        }

        // פונקציה ליצירת טוקן JWT (סינכרונית)
        private string GenerateToken(CustomerDto customer)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, customer.FullName),
                new Claim(ClaimTypes.Email, customer.Email),
                new Claim(ClaimTypes.Role, customer.Role.ToString()),
                new Claim("CustomerId", customer.Id.ToString())
            };

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
