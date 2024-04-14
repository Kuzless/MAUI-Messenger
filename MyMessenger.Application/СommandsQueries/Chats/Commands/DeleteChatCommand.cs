using MediatR;

namespace MyMessenger.Application.СommandsQueries.Chats.Commands
{
    public class DeleteChatCommand : IRequest
    {
        public int Id { get; set; }
        public string OwnerId { get; set; }
        public DeleteChatCommand(string OwnerId, int Id )
        {
            this.OwnerId = OwnerId;
            this.Id = Id;
        }
    }
}
