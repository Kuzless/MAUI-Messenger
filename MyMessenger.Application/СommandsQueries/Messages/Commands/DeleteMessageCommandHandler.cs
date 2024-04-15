using MediatR;
using MyMessenger.Domain.Entities;
using MyMessenger.Domain.Interfaces;

namespace MyMessenger.Application.СommandsQueries.Messages.Commands
{
    public class DeleteMessageCommandHandler : IRequestHandler<DeleteMessageCommand>
    {
        private readonly IUnitOfWork unitOfWork;
        public DeleteMessageCommandHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task Handle(DeleteMessageCommand request, CancellationToken cancellationToken)
        {
            var message = await unitOfWork.GetRepository<Message>().GetById(request.Id);
            unitOfWork.GetRepository<Message>().Delete(message);
            await unitOfWork.SaveAsync();
        }
    }
}
