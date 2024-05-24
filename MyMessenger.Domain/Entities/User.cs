using Microsoft.AspNetCore.Identity;

namespace MyMessenger.Domain.Entities
{
    public class User : IdentityUser
    {
        public string Name { get; set; }
        public string? RefreshToken { get; set; }
        public string? Image { get; set; }
        public DateTime? RefreshTokenExpiryTime { get; set; }
        public virtual ICollection<Chat> Chats { get; set; } = new List<Chat>();
        public virtual ICollection<Message> Messages { get; set; } = new List<Message>(); 
    }
}
