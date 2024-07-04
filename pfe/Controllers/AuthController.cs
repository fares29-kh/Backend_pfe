using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using pfe.config;
using pfe.models;
using pfe.ModelViews;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace projetPFESagemCom.Controllers
{
    [Route("auth")]
    [ApiController]
    [AllowAnonymous]
    public class AuthController : ControllerBase
    {
        private readonly DBContext _db;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly IConfiguration _configuration;

        public AuthController(DBContext db, UserManager<User> userManager, SignInManager<User> signInManager, RoleManager<Role> roleManager,
                IConfiguration configuration)
        {
            _db = db;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register(registerModel model)
        {
            await CreateRoles();
            if (model == null)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                if (EmailExist(model.Email))
                {
                    return BadRequest("Email is used ");
                }
                var user = new User
                {
                    Email = model.Email,
                    UserName = model.Username,
                    PhoneNumber = model.PhoneNumber,
                };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, model.role);
                    return Ok();
                }
            }
            return BadRequest();
        }
        private bool EmailExist(string email)
        {
            if (_db.Users.Any(u => u.Email == email))
            {
                return true;
            }
            return false;
        }
        private async Task CreateRoles()
        {
            if (_roleManager.Roles.Count() < 1)
            {
                var role = new Role
                {
                    Name = "admin"
                };
                await _roleManager.CreateAsync(role);

                role = new Role
                {
                    Name = "user"
                };
                await _roleManager.CreateAsync(role);
                role = new Role
                {
                    Name = "proprietaire",
                };
                await _roleManager.CreateAsync(role);
            }
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(loginModel Model)
        {
            await CreateRoles();
            await CreateAdmin();
            if (Model == null)
            {
                return NotFound();
            }
            var user = await _userManager.FindByEmailAsync(Model.Email);
            if (user == null)
            {
                return NotFound();
            }
            var result = await _userManager.CheckPasswordAsync(user, Model.Password);
            if (result)
            {
                var roleName = await GetRoleNameByUserId(user.Id);
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.NameIdentifier,user.Id),
                    new Claim(ClaimTypes.Role,roleName),
                };
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var token = new JwtSecurityToken(_configuration["Jwt:Issuer"],
                                                 _configuration["Jwt:Issuer"],
                                                 claims,
                                                 expires: DateTime.Now.AddMinutes(30),
                                                 signingCredentials: creds);
                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token)
                });
            }
            else
            {
                return Unauthorized();
            }
        }
        private async Task<string?> GetRoleNameByUserId(string userId)
        {
            var userRole = await _db.UserRoles.FirstOrDefaultAsync(x => x.UserId == userId);
            if (userRole != null)
            {
                return await _db.Roles.Where(x => x.Id == userRole.RoleId).Select(x => x.Name).FirstOrDefaultAsync();
            }
            return null;
        }
        private async Task CreateAdmin()
        {
            var admin = await _userManager.FindByNameAsync("admin");
            if (admin == null)
            {
                var user = new User
                {
                    UserName = "admin",
                    Email = "admin@admin.com",
                    EmailConfirmed = true,
                };
                var result = await _userManager.CreateAsync(user, "Admin123");
                if (result.Succeeded)
                {
                    if (await _roleManager.RoleExistsAsync("admin"))
                    {
                        await _userManager.AddToRoleAsync(user, "admin");
                    }
                }
            }
        }
    }
}
