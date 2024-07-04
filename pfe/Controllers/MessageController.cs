using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using pfe.config;
using pfe.models;
using pfe.modelViews;

namespace pfe.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly DBContext _db;
        public MessageController(DBContext dB) 
        {
            _db = dB;
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<Message>> GetMessage(int id)
        {
            var Message = await _db.Messages.FindAsync(id);
            if (Message == null)
            {
                return NotFound();
            }
            return Ok(Message);
        } 

        [HttpPost]
        public async Task<ActionResult<Message>> AddMessage(Message message)
        {
            _db.Messages.Add(message);
            await _db.SaveChangesAsync();
            return CreatedAtAction(nameof(GetMessage),new {id = message.Id},message);
        }

        [HttpGet]
        [Route("user/{user1}/{user2}")]
        public async Task<ActionResult<List<Message>>> GetUserMessages(string user1 , string user2) 
        {
            return await _db.Messages
                .Where(x => 
                (x.UserName == user1 && x.userId == user2) || (x.UserName == user2 && x.userId == user1)
                ).OrderBy(x=>x.date)
                .ToListAsync();
        }

        [HttpGet]
        [Route("contacts/{userId}")]
        public async Task<ActionResult<List<userModel>>> GetContacts(string userId)
        {
            List<userModel> result = new List<userModel> ();
            List<Message> lst = new List<Message>();
            lst = await _db.Messages.Where(
                x=>x.userId == userId || x.UserName == userId
                )
                .ToListAsync();
            foreach(Message m in lst)
            {
                var user = _db.Users.Where(x => (x.Id == m.userId || x.Id == m.UserName) && x.Id != userId).ToList()[0];
                userModel model = new userModel
                {
                    userName = user.UserName,
                    Id = user.Id,
                };
                int res = result.Where(x=>x.Id == model.Id).Count();
                if (res == 0)
                {
                    result.Add(model);
                }
            }
            return Ok(result);
        }   
    }
}
