using Microsoft.AspNetCore.Identity;

namespace MyMessenger.Domain.Entities
{
    public class User : IdentityUser
    {
        public string Name { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiryTime { get; set; }
        public ICollection<Chat> Chats { get; set; }
        public ICollection<Message> Messages { get; set; }
    }
}
