using MediatR;
using MyMessenger.Application.DTO;
using MyMessenger.Application.DTO.ChatDTOs;

namespace MyMessenger.Application.СommandsQueries.Chats.Queries
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
