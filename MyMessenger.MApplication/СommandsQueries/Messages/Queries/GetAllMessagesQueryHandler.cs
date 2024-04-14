using AutoMapper;
using MediatR;
using MyMessenger.Domain.Interfaces;
using MyMessenger.MApplication.DTO;
using MyMessenger.MApplication.DTO.MessagesDTOs;
using MyMessenger.Domain.Entities;

namespace MyMessenger.MApplication.СommandsQueries.Messages.Queries
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
            int pageNumber = query.PageNumber ?? 1;
            int pageSize = query.PageSize ?? 10;
            int skipSize = (pageNumber - 1) * pageSize;
            string subs = query.Subs ?? "";

            var dbQuery = unitOfWork.GetRepository<Message>().GetAll();
            var queryResult = await unitOfWork.GetRepository<Message>().FilterByQuery(dbQuery, query.Sort, skipSize, pageSize, subs, query.UserId);
            var resultMapped = mapper.Map<IEnumerable<MessageDTO>>(queryResult);

            var numAllPages = unitOfWork.GetRepository<Message>().GetNumberOfRecords();
            var numPages = (int)Math.Ceiling((double)numAllPages / pageSize);

            DataForGridDTO<MessageDTO> result = new DataForGridDTO<MessageDTO>() { Data = resultMapped, NumberOfPages = numPages };
            return result;
        }
    }
}
