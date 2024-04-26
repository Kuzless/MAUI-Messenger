using MediatR;
using MyMessenger.Application.DTO;
using MyMessenger.Application.DTO.MessagesDTOs;

namespace MyMessenger.Application.СommandsQueries.Messages.Commands
{
    public class UpdateMessageCommand : IRequest<ResponseDTO>
    {
        public MessageDTO MessageDTO { get; set; }
        public string UserId { get; set; }
        public UpdateMessageCommand(MessageDTO MessageDTO, string UserId)
        {
            this.MessageDTO = MessageDTO;
            this.UserId = UserId;
        }
    }
}
