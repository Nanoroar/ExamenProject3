using ExamenProject3.Data;
using ExamenProject3.Forms;
using ExamenProject3.Models.User;
using ExamenProject3.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ExamenProject3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService userService;
        private readonly DataContext _context;
        private readonly IConfiguration _configuration;
        public UsersController(IUserService userService, DataContext context, IConfiguration configuration)
        {
            this.userService = userService;
            _context = context;
            _configuration = configuration; 
        }

        [HttpGet]
        public async Task<ActionResult> Get()
        {
            return Ok(await userService.GetAllAsync());
        }


        [HttpGet("{id}")]
        public async Task<ActionResult> Get(int id)
        {
            if (await userService.GetUserByIdAsync(id) == null)
                return NotFound();
            return Ok(await userService.GetUserByIdAsync(id));
        }

        [HttpPost("signup")]
        public async Task<ActionResult> Create(SignUpForm form)
        {
            if(await userService.CreateUserAsync(form))
                return Ok("User Created Successfully ");   
            
            return BadRequest("This Email is Taken");
        }

        [HttpPost("Signin")]
        public async Task<IActionResult> SignIn(SignInForm form)
        {
            if(string.IsNullOrEmpty(form.Email) || string.IsNullOrEmpty(form.Password))
            {
                return new BadRequestObjectResult("Email and password must be filled!");
            }

            var userEntity = await _context.Users.FirstOrDefaultAsync(u => u.Email == form.Email);
            if (userEntity == null)
                return new BadRequestObjectResult("Incorrect Email or Password");

            if(!userEntity.ComapareSecurePassword(form.Password))
                return new BadRequestObjectResult("Incorrect Email or Password");

            var tokenhandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, userEntity.Email),
                    new Claim(ClaimTypes.Email, userEntity.Email),
                    new Claim("UserId", userEntity.Id.ToString()),
                    new Claim("ApiKey", _configuration.GetValue<string>("ApiKeys:ApiKey"))
                }),

                Expires = DateTime.Now.AddDays(1),

               
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetValue<string>("ApiKeys:SecretKey"))),
                    SecurityAlgorithms.HmacSha512Signature
                    )
            };

            var accessToken = tokenhandler.WriteToken(tokenhandler.CreateToken(tokenDescriptor));
            return new OkObjectResult(accessToken); 
        }

       
        [Authorize]

        [HttpPut("update/{id}")]

        public async Task<ActionResult<UserUpdate>> Update(int id, UserUpdate userUpdate)
        {
            var userToUpdate = await userService.UpdateAsync(id, userUpdate);
            if (userToUpdate == null)
                return NotFound();
            return Ok(userToUpdate);
        }

        
        [HttpDelete("Delete/{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            if (!await userService.DeleteAsync(id))
                return NotFound();
            return Ok();
        }
    }
}
