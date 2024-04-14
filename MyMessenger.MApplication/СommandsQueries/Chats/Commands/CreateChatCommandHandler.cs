using MediatR;
using MyMessenger.Domain.Interfaces;
using MyMessenger.MApplication.Services.Interfaces;

namespace MyMessenger.MApplication.СommandsQueries.Chats.Commands
{
    public class CreateChatCommandHandler : IRequestHandler<CreateChatCommand>
    {
        private readonly IUnitOfWork unitOfWork;
        public CreateChatCommandHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task Handle(CreateChatCommand request, CancellationToken cancellationToken)
        {
            await unitOfWork.Chat.AddChat(request.Name, request.OwnerId, request.User);
            await unitOfWork.SaveAsync();
        }
    }
}
