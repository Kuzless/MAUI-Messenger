using AutoMapper;
using MediatR;
using MyMessenger.Application.DTO.MessagesDTOs;
using MyMessenger.Domain.Interfaces;

namespace MyMessenger.Application.СommandsQueries.Messages.Queries
{
    public class GetMessageByIdQueryHandler : IRequestHandler<GetMessageByIdQuery, MessageDTO>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        public GetMessageByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task<MessageDTO> Handle(GetMessageByIdQuery request, CancellationToken cancellationToken)
        {
            var message = await unitOfWork.Message.GetMessageById(request.Id);
            return mapper.Map<MessageDTO>(message);
        }
    }
}
