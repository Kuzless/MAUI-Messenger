using MediatR;

namespace MyMessenger.Application.СommandsQueries.Messages.Commands
{
    public class CreateMessageCommand : IRequest<int>
    {
        public string UserId { get; set; }
        public int ChatId { get; set; }
        public string Text { get; set; }
        public DateTime DateTime { get; set; }
        public CreateMessageCommand(string UserId, int ChatId, string Text, DateTime DateTime)
        {
            this.Text = Text;
            this.UserId = UserId;
            this.ChatId = ChatId;
            this.DateTime = DateTime;
        }
    }
}
