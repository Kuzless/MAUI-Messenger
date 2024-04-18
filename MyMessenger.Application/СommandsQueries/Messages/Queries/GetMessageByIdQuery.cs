using MediatR;
using MyMessenger.Application.DTO.MessagesDTOs;
using MyMessenger.Application.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyMessenger.Application.СommandsQueries.Messages.Queries
{
    public class GetMessageByIdQuery : IRequest<MessageDTO>
    {
        public int Id { get; set; }
        public GetMessageByIdQuery(int Id)
        {
            this.Id = Id;
        }
    }
}
