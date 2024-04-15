using MediatR;
using MyMessenger.Domain.Entities;
using MyMessenger.Domain.Interfaces;

namespace MyMessenger.Application.СommandsQueries.Chats.Commands
{
    public class JoinChatCommandHandler : IRequestHandler<JoinChatCommand>
    {
        private readonly IUnitOfWork unitOfWork;
        public JoinChatCommandHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task Handle(JoinChatCommand request, CancellationToken cancellationToken)
        {
            var chat = await unitOfWork.GetRepository<Chat>().GetById(request.Id);
            unitOfWork.Chat.AddMember(chat, request.User);
            await unitOfWork.SaveAsync();
        }
    }
}
