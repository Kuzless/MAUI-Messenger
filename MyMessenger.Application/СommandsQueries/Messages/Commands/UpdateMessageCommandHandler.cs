using AutoMapper;
using MediatR;
using MyMessenger.Domain.Entities;
using MyMessenger.Domain.Interfaces;

namespace MyMessenger.Application.СommandsQueries.Messages.Commands
{
    public class UpdateMessageCommandHandler : IRequestHandler<UpdateMessageCommand>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        public UpdateMessageCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.mapper = mapper;
            this.unitOfWork = unitOfWork;
        }
        public async Task Handle(UpdateMessageCommand request, CancellationToken cancellationToken)
        {
            var message = mapper.Map<Message>(request.MessageDTO);
            message.UserId = request.UserId;
            unitOfWork.GetRepository<Message>().Update(message);
            await unitOfWork.SaveAsync();
        }
    }
}
