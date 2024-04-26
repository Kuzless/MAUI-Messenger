using AutoMapper;
using MediatR;
using MyMessenger.Application.DTO;
using MyMessenger.Domain.Entities;
using MyMessenger.Domain.Interfaces;

namespace MyMessenger.Application.СommandsQueries.Messages.Commands
{
    public class UpdateMessageCommandHandler : IRequestHandler<UpdateMessageCommand, ResponseDTO>
    {
        private readonly IUnitOfWork unitOfWork;
        public UpdateMessageCommandHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<ResponseDTO> Handle(UpdateMessageCommand request, CancellationToken cancellationToken)
        {
            var message = await unitOfWork.Message.GetById(request.MessageDTO.Id);
            if (message.UserId == request.UserId)
            {
                message.Text = request.MessageDTO.Text;
                unitOfWork.GetRepository<Message>().Update(message);
                await unitOfWork.SaveAsync();
                return new ResponseDTO() { isSuccessful = true };
            }
            return new ResponseDTO() { isSuccessful = false };
        }
    }
}
