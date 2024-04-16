using AutoMapper;
using MediatR;
using MyMessenger.Domain.Interfaces;
using MyMessenger.Application.DTO;
using MyMessenger.Application.DTO.MessagesDTOs;
using MyMessenger.Domain.Entities;

namespace MyMessenger.Application.СommandsQueries.Messages.Queries
{
    public class GetAllMessagesQueryHandler : IRequestHandler<GetAllMessagesQuery, DataForGridDTO<MessageDTO>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        public GetAllMessagesQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }
        public async Task<DataForGridDTO<MessageDTO>> Handle(GetAllMessagesQuery query, CancellationToken cancellationToken)
        {
            int skipSize = (query.PageNumber - 1) * query.PageSize;
            try
            {
                var dbQuery = unitOfWork.GetRepository<Message>().GetAll();
                var queryResult = await unitOfWork.GetRepository<Message>().FilterByQuery(dbQuery, query.Sort, skipSize, query.PageSize, query.Subs, query.UserId);
                var resultMapped = mapper.Map<IEnumerable<MessageDTO>>(queryResult);

                var numAllPages = unitOfWork.GetRepository<Message>().GetNumberOfRecords();
                var numPages = (int)Math.Ceiling((double)numAllPages / query.PageSize);

                DataForGridDTO<MessageDTO> result = new DataForGridDTO<MessageDTO>() { Data = resultMapped, NumberOfPages = numPages };
                return result;
            } catch (Exception ex) 
            {
                return new DataForGridDTO<MessageDTO>();
            }
        }
    }
}
