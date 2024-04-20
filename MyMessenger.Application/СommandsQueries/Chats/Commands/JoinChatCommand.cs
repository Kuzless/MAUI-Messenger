using MediatR;
namespace MyMessenger.Application.СommandsQueries.Chats.Commands
{
    public class JoinChatCommand : IRequest
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public JoinChatCommand(string UserName, int Id)
        {
            this.UserName = UserName;
            this.Id = Id;
        }
    }
}
