using AutoMapper;
using MediatR;
using MyMessenger.Application.DTO;
using MyMessenger.Application.DTO.MessagesDTOs;
using MyMessenger.Domain.Entities;
using MyMessenger.Domain.Interfaces;

namespace MyMessenger.Application.СommandsQueries.Messages.Queries
{
    public class GetMessagesByChatIdQueryHandler : IRequestHandler<GetMessagesByChatIdQuery, DataForGridDTO<MessageDTO>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        public GetMessagesByChatIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }
        public async Task<DataForGridDTO<MessageDTO>> Handle(GetMessagesByChatIdQuery query, CancellationToken cancellationToken)
        {
            int skipSize = (query.PageNumber - 1) * query.PageSize;
            try
            {
                var dbQuery = unitOfWork.Message.GetMessagesByChatId(query.ChatId);
                var queryResult = await unitOfWork.GetRepository<Message>().FilterByQuery(dbQuery, query.Sort, skipSize, query.PageSize, query.Subs);
                var resultMapped = mapper.Map<IEnumerable<MessageDTO>>(queryResult.Keys.First());

                var numPages = (queryResult.Values.First() + query.PageSize - 1) / query.PageSize;

                DataForGridDTO<MessageDTO> result = new DataForGridDTO<MessageDTO>() { Data = resultMapped, NumberOfPages = numPages };
                return result;
            }
            catch (Exception ex)
            {
                return new DataForGridDTO<MessageDTO>();
            }
        }
    }
}
