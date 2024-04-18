using AutoMapper;
using MediatR;
using MyMessenger.Application.DTO.MessagesDTOs;
using MyMessenger.Application.DTO;
using MyMessenger.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            return mapper .Map<MessageDTO>(unitOfWork.Message.GetMessageById(request.Id));
        }
    }
}
