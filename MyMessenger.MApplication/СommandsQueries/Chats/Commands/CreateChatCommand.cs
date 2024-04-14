using MediatR;
using MyMessenger.Domain.Entities;

namespace MyMessenger.MApplication.СommandsQueries.Chats.Commands
{
    public class CreateChatCommand : IRequest
    {
        public string OwnerId { get; set; }
        public string Name { get; set; }
        public User User { get; set; }
        public CreateChatCommand(string OwnerId, string Name, User User)
        {
            this.OwnerId = OwnerId;
            this.Name = Name;
            this.User = User;
        }
    }
}
