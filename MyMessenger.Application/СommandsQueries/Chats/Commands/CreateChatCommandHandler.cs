using MediatR;
using MyMessenger.Application.Services.Interfaces;
using MyMessenger.Domain.Interfaces;

namespace MyMessenger.Application.СommandsQueries.Chats.Commands
{
    public class CreateChatCommandHandler : IRequestHandler<CreateChatCommand>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IUserService userService;
        public CreateChatCommandHandler(IUnitOfWork unitOfWork, IUserService userService)
        {
            this.unitOfWork = unitOfWork;
            this.userService = userService;
        }
        public async Task Handle(CreateChatCommand request, CancellationToken cancellationToken)
        {
            var user = await userService.GetUserById(request.UserId);
            await unitOfWork.Chat.AddChat(request.Name, request.OwnerId, user);
            await unitOfWork.SaveAsync();
        }
    }
}
