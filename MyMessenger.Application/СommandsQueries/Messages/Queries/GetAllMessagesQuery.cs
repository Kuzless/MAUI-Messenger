using MediatR;
using MyMessenger.Application.DTO;
using MyMessenger.Application.DTO.MessagesDTOs;

namespace MyMessenger.Application.СommandsQueries.Messages.Queries
{
    public class GetAllMessagesQuery : IRequest<DataForGridDTO<MessageDTO>>
    {
        public Dictionary<string, bool> Sort { get; set; }
        public string UserId { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string Subs { get; set; }
        public GetAllMessagesQuery(Dictionary<string, bool> Sort, int PageNumber, int PageSize, string Subs, string UserId)
        {
            this.PageNumber = PageNumber;
            this.Sort = Sort;
            this.Subs = Subs;
            this.PageSize = PageSize;
            this.UserId = UserId;
        }
    }
}
