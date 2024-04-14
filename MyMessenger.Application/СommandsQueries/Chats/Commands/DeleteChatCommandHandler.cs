using MediatR;
using MyMessenger.Domain.Entities;
using MyMessenger.Domain.Interfaces;

namespace MyMessenger.Application.СommandsQueries.Chats.Commands
{
    public class DeleteChatCommandHandler : IRequestHandler<DeleteChatCommand>
    {
        private readonly IUnitOfWork unitOfWork;
        public DeleteChatCommandHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task Handle(DeleteChatCommand request, CancellationToken cancellationToken)
        {
            var chat = await unitOfWork.GetRepository<Chat>().GetById(request.Id);
            if (chat.OwnerId == request.OwnerId) 
            {
                unitOfWork.GetRepository<Chat>().Delete(chat);
                await unitOfWork.SaveAsync();
            }
        }
    }
}
