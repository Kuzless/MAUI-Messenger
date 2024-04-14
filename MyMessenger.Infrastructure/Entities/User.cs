using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyMessenger.Domain.Entities
{
    public class User : IdentityUser
    {
        public string Name { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiryTime { get; set; }
        public IEnumerable<Chat> Chats { get; set; }
        public IEnumerable<Message> Messages { get; set; }
    }
}
