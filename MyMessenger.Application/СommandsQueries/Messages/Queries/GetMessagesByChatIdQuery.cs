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
    public class GetMessagesByChatIdQuery : IRequest<DataForGridDTO<MessageDTO>>
    {
        public int ChatId { get; set; }
        public Dictionary<string, bool> Sort { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string Subs { get; set; }
        public GetMessagesByChatIdQuery(int PageNumber, int PageSize, string Subs, int ChatId)
        {
            this.PageNumber = PageNumber;
            Sort = new Dictionary<string, bool>() { { "DateTime" , false } };
            this.Subs = Subs;
            this.PageSize = PageSize;
            this.ChatId = ChatId;
        }
    }
}
