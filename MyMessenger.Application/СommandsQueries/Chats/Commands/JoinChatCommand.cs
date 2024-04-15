using MediatR;
using MyMessenger.Domain.Entities;

namespace MyMessenger.Application.СommandsQueries.Chats.Commands
{
    public class JoinChatCommand : IRequest
    {
        public int Id { get; set; }
        public User User { get; set; }
        public JoinChatCommand(User User, int Id)
        {
            this.User = User;
            this.Id = Id;
        }
    }
}
