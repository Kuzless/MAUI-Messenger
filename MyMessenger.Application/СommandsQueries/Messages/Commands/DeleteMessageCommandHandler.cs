using MediatR;
using MyMessenger.Application.DTO;
using MyMessenger.Domain.Entities;
using MyMessenger.Domain.Interfaces;

namespace MyMessenger.Application.СommandsQueries.Messages.Commands
{
    public class DeleteMessageCommandHandler : IRequestHandler<DeleteMessageCommand, ResponseDTO>
    {
        private readonly IUnitOfWork unitOfWork;
        public DeleteMessageCommandHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<ResponseDTO> Handle(DeleteMessageCommand request, CancellationToken cancellationToken)
        {
            var message = await unitOfWork.GetRepository<Message>().GetById(request.Id);
            if (message.UserId == request.UserId)
            {
                unitOfWork.GetRepository<Message>().Delete(message);
                await unitOfWork.SaveAsync();
                return new ResponseDTO() { isSuccessful = true };
            }
            return new ResponseDTO() { isSuccessful = false };
        }
    }
}
