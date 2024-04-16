using MediatR;
using MyMessenger.Application.Services.Interfaces;
using MyMessenger.Domain.Entities;
using MyMessenger.Domain.Interfaces;

namespace MyMessenger.Application.СommandsQueries.Chats.Commands
{
    public class JoinChatCommandHandler : IRequestHandler<JoinChatCommand>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IUserService userService;
        public JoinChatCommandHandler(IUnitOfWork unitOfWork, IUserService userService)
        {
            this.unitOfWork = unitOfWork;
            this.userService = userService;
        }
        public async Task Handle(JoinChatCommand request, CancellationToken cancellationToken)
        {
            var user = await userService.GetUserById(request.UserId);
            var chat = await unitOfWork.GetRepository<Chat>().GetById(request.Id);
            unitOfWork.Chat.AddMember(chat, user);
            await unitOfWork.SaveAsync();
        }
    }
}
