using AutoMapper;
using MediatR;
using MyMessenger.Domain.Interfaces;

namespace MyMessenger.Application.СommandsQueries.Messages.Commands
{

    internal class CreateMessageCommandHandler : IRequestHandler<CreateMessageCommand, int>
    {
        private readonly IUnitOfWork unitOfWork;
        public CreateMessageCommandHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<int> Handle(CreateMessageCommand request, CancellationToken cancellationToken)
        {
            var addedMessage = await unitOfWork.Message.AddMessage(request.UserId, request.ChatId, request.Text);
            await unitOfWork.SaveAsync();

            return addedMessage.Id;
        }
    }
}
