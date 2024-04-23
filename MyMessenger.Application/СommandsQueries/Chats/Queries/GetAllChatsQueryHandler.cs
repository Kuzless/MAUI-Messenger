using MediatR;
using MyMessenger.Application.DTO;
using MyMessenger.Application.DTO.ChatDTOs;
using AutoMapper;
using MyMessenger.Domain.Entities;
using MyMessenger.Domain.Interfaces;

namespace MyMessenger.Application.СommandsQueries.Chats.Queries
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
            int skipSize = (query.PageNumber - 1) * query.PageSize;

            var dbQuery = unitOfWork.Chat.GetChatsByUserId(query.UserId);
            var queryResult = await unitOfWork.GetRepository<Chat>().FilterByQuery(dbQuery, query.Sort, skipSize, query.PageSize, query.Subs);
            var resultMapped = mapper.Map<IEnumerable<ChatDTO>>(queryResult.Keys.First());

            var numPages = (queryResult.Values.First() + query.PageSize - 1) / query.PageSize;

            DataForGridDTO<ChatDTO> result = new DataForGridDTO<ChatDTO>() { Data = resultMapped, NumberOfPages = numPages };
            return result;
        }
    }
}
