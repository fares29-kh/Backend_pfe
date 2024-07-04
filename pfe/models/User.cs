using Microsoft.AspNetCore.Identity;

namespace pfe.models
{
    public class User : IdentityUser
    {
        public User() 
        { 
            Messages = new HashSet<Message>();
        }
        public ICollection<Message>? Messages { get; set; }
    }
}
