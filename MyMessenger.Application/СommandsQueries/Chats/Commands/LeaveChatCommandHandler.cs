using MediatR;
using MyMessenger.Application.Services.Interfaces;
using MyMessenger.Domain.Entities;
using MyMessenger.Domain.Interfaces;

namespace MyMessenger.Application.СommandsQueries.Chats.Commands
{
    public class LeaveChatCommandHandler : IRequestHandler<LeaveChatCommand>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IUserService userService;
        public LeaveChatCommandHandler(IUnitOfWork unitOfWork, IUserService userService)
        {
            this.unitOfWork = unitOfWork;
            this.userService = userService;
        }
        public async Task Handle(LeaveChatCommand request, CancellationToken cancellationToken)
        {
            var user = await userService.GetUserById(request.UserId);
            var chat = await unitOfWork.Chat.GetChatById(request.Id);
            unitOfWork.Chat.DeleteMember(chat, user);
            if (chat.OwnerId == user.Id)
            {
                unitOfWork.GetRepository<Chat>().Delete(chat);
            }
            await unitOfWork.SaveAsync();
        }
    }
}
