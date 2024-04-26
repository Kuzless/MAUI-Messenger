using AutoMapper;
using MediatR;
using MyMessenger.Domain.Entities;
using MyMessenger.Domain.Interfaces;

namespace MyMessenger.Application.СommandsQueries.Messages.Commands
{

    public class CreateMessageCommandHandler : IRequestHandler<CreateMessageCommand, int>
    {
        private readonly IUnitOfWork unitOfWork;
        public CreateMessageCommandHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<int> Handle(CreateMessageCommand request, CancellationToken cancellationToken)
        {
            Message message = new Message() { UserId = request.UserId, ChatId = request.ChatId, Text = request.Text, DateTime = request.DateTime };
            var addedMessage = await unitOfWork.Message.AddMessage(message);
            await unitOfWork.SaveAsync();

            return addedMessage.Id;
        }
    }
}
