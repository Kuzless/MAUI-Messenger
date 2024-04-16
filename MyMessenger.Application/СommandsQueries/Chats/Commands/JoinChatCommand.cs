using MediatR;
using MyMessenger.Domain.Entities;

namespace MyMessenger.Application.СommandsQueries.Chats.Commands
{
    public class JoinChatCommand : IRequest
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public JoinChatCommand(string UserId, int Id)
        {
            this.UserId = UserId;
            this.Id = Id;
        }
    }
}
