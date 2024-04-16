using MediatR;

namespace MyMessenger.Application.СommandsQueries.Chats.Commands
{
    public class CreateChatCommand : IRequest
    {
        public string OwnerId { get; set; }
        public string Name { get; set; }
        public string UserId { get; set; }
        public CreateChatCommand(string OwnerId, string Name, string UserId)
        {
            this.OwnerId = OwnerId;
            this.Name = Name;
            this.UserId = UserId;
        }
    }
}
