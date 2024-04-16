using MediatR;
using MyMessenger.Domain.Entities;

namespace MyMessenger.Application.СommandsQueries.Chats.Commands
{
    public class LeaveChatCommand : IRequest
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public LeaveChatCommand(string UserId, int Id)
        {
            this.UserId = UserId;
            this.Id = Id;
        }
    }
}
