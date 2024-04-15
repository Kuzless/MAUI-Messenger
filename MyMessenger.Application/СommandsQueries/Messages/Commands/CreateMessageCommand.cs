using MediatR;

namespace MyMessenger.Application.СommandsQueries.Messages.Commands
{
    public class CreateMessageCommand : IRequest
    {
        public string UserId { get; set; }
        public int ChatId { get; set; }
        public string Text { get; set; }
        public CreateMessageCommand(string UserId, int ChatId, string Text)
        {
            this.Text = Text;
            this.UserId = UserId;
            this.ChatId = ChatId;
        }
    }
}
