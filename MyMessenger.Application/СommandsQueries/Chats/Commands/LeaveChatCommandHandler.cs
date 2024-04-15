using MediatR;
using MyMessenger.Domain.Entities;
using MyMessenger.Domain.Interfaces;

namespace MyMessenger.Application.СommandsQueries.Chats.Commands
{
    public class LeaveChatCommandHandler : IRequestHandler<LeaveChatCommand>
    {
        private readonly IUnitOfWork unitOfWork;
        public LeaveChatCommandHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task Handle(LeaveChatCommand request, CancellationToken cancellationToken)
        {
            var chat = await unitOfWork.Chat.GetChatById(request.Id);
            unitOfWork.Chat.DeleteMember(chat, request.User);
            if (chat.OwnerId == request.User.Id)
            {
                unitOfWork.GetRepository<Chat>().Delete(chat);
            }
            await unitOfWork.SaveAsync();
        }
    }
}
