
using MediatR;
using MyMessenger.MApplication.DTO.MessagesDTOs;
using MyMessenger.MApplication.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyMessenger.MApplication.DTO.ChatDTOs;

namespace MyMessenger.MApplication.СommandsQueries.Chats.Queries
{
    public class GetAllChatsQuery : IRequest<DataForGridDTO<ChatDTO>>
    {
        public Dictionary<string, bool>? Sort { get; set; }
        public string? UserId { get; set; }
        public int? PageNumber { get; set; }
        public int? PageSize { get; set; }
        public string? Subs { get; set; }
        public GetAllChatsQuery(Dictionary<string, bool>? Sort, int? PageNumber, int? PageSize, string? Subs, string? UserId)
        {
            this.PageNumber = PageNumber;
            this.Sort = Sort;
            this.Subs = Subs;
            this.PageSize = PageSize;
            this.UserId = UserId;
        }
    }
}
