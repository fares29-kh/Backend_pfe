using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using pfe.config;
using pfe.models;
using pfe.ModelViews;
using System.ComponentModel;

namespace pfe.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly DBContext _db;
        private readonly UserManager<User> _userManager;
        public UserController(DBContext db, UserManager<User> userManager)
        {
            _db = db;
            _userManager = userManager;
        }
        [HttpGet]
        public async Task<List<User>> GetUsersAsync()
        {
            return await _db.Users.ToListAsync();
        }

        [HttpGet]
        [Route("user/{id}")]
        public async Task<User> GetUserInfo(string id)
        {
            return await _db.Users.FindAsync(id);
        }

        [HttpGet]
        [Route("listUser")]
        public async Task<List<User>> GetListUsers()
        {
            List<User> result = new List<User>();
            Role? roleId = await _db.Roles.FirstOrDefaultAsync(x => x.Name == "user");
            if(roleId != null)
            {
                var users = _db.UserRoles.Where(x=>x.RoleId == roleId.Id).ToList();
                foreach(var user in users)
                {
                    User tmp = _db.Users.Where(x => x.Id == user.UserId).ToList()[0];
                    result.Add(tmp);
                }
            }
            return result;

        }
       [HttpDelete]
       [Route("{nomuser}")]
       public async Task<IActionResult> DeleteUser(string nomuser)
        {
            var maison = await _db.Users.FirstOrDefaultAsync(m => m.UserName == nomuser);
          if (maison == null)
          {
                return NotFound();
         }
           _db.Users.Remove(maison);
         await _db.SaveChangesAsync();
          return NoContent();
       }

        [HttpGet]
        [Route("listOwner")]
        public async Task<List<User>> GetListOwners()
        {
            List<User> result = new List<User>();
            Role? roleId = await _db.Roles.FirstOrDefaultAsync(x => x.Name == "proprietaire");
            if (roleId != null)
            {
                var users = _db.UserRoles.Where(x => x.RoleId == roleId.Id).ToList();
                foreach (var user in users)
                {
                    User tmp = _db.Users.Where(x => x.Id == user.UserId).ToList()[0];
                    result.Add(tmp);
                }
            }
            return result;
        }
        [HttpPut]
        [Route("{id}")]
        public async  Task<ActionResult> updateUser(int id, registerModel userModel)
        {
            var user = await _db.Users.FindAsync(id);
            if(user == null)
            {
                return NotFound();
            }
            user.UserName = userModel.Username;
            user.PhoneNumber = userModel.PhoneNumber;
            var hashedpassword = BCrypt.Net.BCrypt.HashPassword(userModel.Password);
            user.PasswordHash = hashedpassword;
            string? currentRole = await GetRoleNameByUserId(user.Id);
            if(currentRole != null)
            {
                await _userManager.RemoveFromRoleAsync(user, currentRole);
                await _userManager.AddToRoleAsync(user, userModel.role);
            }
            await _db.SaveChangesAsync();
            return Ok();
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

        [HttpGet]
        [Route("countUsersByRole")]
        public async Task<int[]> GetCountUsersByRole()
        {
            int[] countByRole = new int[2];

            Role? proprietaireRole = await _db.Roles.FirstOrDefaultAsync(x => x.Name == "proprietaire");
            Role? userRole = await _db.Roles.FirstOrDefaultAsync(x => x.Name == "user");

            if (proprietaireRole != null)
            {
                countByRole[0] = await _db.UserRoles.CountAsync(x => x.RoleId == proprietaireRole.Id);
            }

            if (userRole != null)
            {
                countByRole[1] = await _db.UserRoles.CountAsync(x => x.RoleId == userRole.Id);
            }

            return countByRole;
        }

    }
}
