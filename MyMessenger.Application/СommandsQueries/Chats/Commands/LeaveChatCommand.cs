using MediatR;
using MyMessenger.Domain.Entities;

namespace MyMessenger.Application.СommandsQueries.Chats.Commands
{
    public class LeaveChatCommand : IRequest
    {
        public int Id { get; set; }
        public User User { get; set; }
        public LeaveChatCommand(User User, int Id)
        {
            this.User = User;
            this.Id = Id;
        }
    }
}
