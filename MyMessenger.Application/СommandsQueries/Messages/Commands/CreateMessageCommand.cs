using MediatR;

namespace MyMessenger.Application.СommandsQueries.Messages.Commands
{
    public class CreateMessageCommand : IRequest
    {
        public int? Id { get; set; }
        public string UserId { get; set; }
        public int ChatId { get; set; }
        public string Text { get; set; }
        public CreateMessageCommand(int Id, string UserId, int ChatId, string Text)
        {
            this.Text = Text;
            this.Id = Id;
            this.UserId = UserId;
            this.ChatId = ChatId;
        }
    }
}
