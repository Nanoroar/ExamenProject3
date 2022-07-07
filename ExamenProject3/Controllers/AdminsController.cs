using ExamenProject3.Data;
using ExamenProject3.Models.Admin;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace ExamenProject3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminsController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IConfiguration _configuration;

        public AdminsController(DataContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration; 
        }

        [HttpPost("register")]

        public async Task<IActionResult> RigisterAdmin(AdminDto request)
        {
            var admin = await _context.Admins.FirstOrDefaultAsync(a => a.Email == request.Email);
            if(admin != null)
            {
                return BadRequest("This Email Address is Taken");
            }

            CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);

            admin = new Admin
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt

            };
            _context.Admins.Add(admin);   
            await _context.SaveChangesAsync();  
            return Ok("Admin created Successfully");
        }

        [HttpPost("adminlogin")]
        public async Task<IActionResult> Login(AdminLogin request)
        {
            var admin = await _context.Admins.FirstOrDefaultAsync(c => c.Email == request.Email);
            if (admin == null)
                return NotFound();
            if (!VerifyPaswordHash(request.Password, admin.PasswordHash, admin.PasswordSalt))
                return BadRequest("Wrong email or password");
            string token = CreateToken(admin);
            string adminkey = _configuration.GetValue<string>("ApiKeys:AdminApiKey");
            var admenAuthentication = new { adminkey, token };  
            return Ok(admenAuthentication);
        }


        // Mehtod Create Token

        private string CreateToken(Admin admin)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, admin.FirstName + " " + admin.LastName),
                new Claim(ClaimTypes.Role, "Admin") ,
                new Claim(ClaimTypes.Email, admin.Email),   
            };

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_configuration.GetSection("SecretKey").Value));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: credentials
                );
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt;

        }

        // Method create passwordHash 

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        //Method verify passwordHash
        private bool VerifyPaswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computeHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computeHash.SequenceEqual(passwordHash);
            }
        }
    }
}
