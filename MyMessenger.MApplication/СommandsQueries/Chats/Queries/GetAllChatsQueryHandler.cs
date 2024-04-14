using MediatR;
using MyMessenger.MApplication.DTO;
using MyMessenger.MApplication.DTO.ChatDTOs;
using AutoMapper;
using MyMessenger.Domain.Entities;
using MyMessenger.Domain.Interfaces;

namespace MyMessenger.MApplication.СommandsQueries.Chats.Queries
{
    public class GetAllChatsQueryHandler : IRequestHandler<GetAllChatsQuery, DataForGridDTO<ChatDTO>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        public GetAllChatsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }
        public async Task<DataForGridDTO<ChatDTO>> Handle(GetAllChatsQuery query, CancellationToken cancellationToken)
        {
            int pageNumber = query.PageNumber ?? 1;
            int pageSize = query.PageSize ?? 10;
            int skipSize = (pageNumber - 1) * pageSize;
            string subs = query.Subs ?? "";

            var dbQuery = unitOfWork.Chat.GetChatsByUserId(query.UserId);
            var queryResult = await unitOfWork.GetRepository<Chat>().FilterByQuery(dbQuery, query.Sort, skipSize, pageSize, subs);
            var resultMapped = mapper.Map<IEnumerable<ChatDTO>>(queryResult);

            var numAllPages = unitOfWork.GetRepository<Chat>().GetNumberOfRecords();
            var numPages = (int)Math.Ceiling((double)numAllPages / pageSize);

            DataForGridDTO<ChatDTO> result = new DataForGridDTO<ChatDTO>() { Data = resultMapped, NumberOfPages = numPages };
            return result;
        }
    }
}
