using MediatR;
using MyMessenger.Domain.Interfaces;

namespace MyMessenger.Application.СommandsQueries.Messages.Commands
{
    internal class CreateMessageCommandHandler : IRequestHandler<CreateMessageCommand>
    {
        private readonly IUnitOfWork unitOfWork;
        public CreateMessageCommandHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task Handle(CreateMessageCommand request, CancellationToken cancellationToken)
        {
            await unitOfWork.Message.AddMessage(request.UserId, request.ChatId, request.Text);
            await unitOfWork.SaveAsync();
        }
    }
}
