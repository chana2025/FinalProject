using Common.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Service.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MyProject.Controllers
{

    //קיים כאן התוקן צריך  להפריד מה ללוגין  ומה לסיין אפ   ןמה לקסטומר
    //Authorize where is exists? check in program
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

        // POST api/login (רישום משתמש חדש)


        // POST api/login/login (כניסה למערכת)
        [HttpPost("login")]
        public IActionResult Login([FromForm] UserLogin userLogin)
        {
            var user = Authenticate(userLogin);
            if (user == null)
                return Unauthorized("User not found.");

            var token = GenerateToken(user);
            return Ok(new { token });
        }

        // PUT api/login/{id} (עדכון לקוח קיים)
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromForm] CustomerDto customer)
        {
            var existingCustomer = _service.GetById(id);
            if (existingCustomer == null)
                return NotFound("Customer not found.");

            customer.Id = id; // להבטיח שה-ID נשמר נכון
            _service.UpdateItem(id, customer); // שלח את ה-ID כ-string
            return Ok("Customer updated successfully.");
        }
        //check if need delete for login
        // DELETE api/login/{id} (מחיקת לקוח)
        //[HttpDelete("{id}")]
        //public IActionResult Delete(int id)
        //{
        //    var customer = _service.GetById(id);
        //    if (customer == null)
        //        return NotFound("Customer not found.");

        //    _service.DeleteItem(id); // שלח את ה-ID כ-string
        //    return Ok("Customer deleted successfully.");
        //}

        // פונקציית אימות
        private CustomerDto Authenticate(UserLogin userLogin)
        {
            var email = userLogin.Email.Trim().ToLower();
            var password = userLogin.Password.Trim();

            return _service.GetAll()
                .FirstOrDefault(u => u.Email.Trim().ToLower() == email &&
                                     u.Password.Trim() == password);
        }



        // פונקציה ליצירת טוקן JWT
        private string GenerateToken(CustomerDto customer)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
    {
        new Claim(ClaimTypes.Name, customer.FullName),
        new Claim(ClaimTypes.Email, customer.Email),
        new Claim(ClaimTypes.Role, customer.Role.ToString()),
        new Claim("CustomerId", customer.Id.ToString()) // ✅ זה מה שמאפשר לנו להציג את התמונה ב־Frontend
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

