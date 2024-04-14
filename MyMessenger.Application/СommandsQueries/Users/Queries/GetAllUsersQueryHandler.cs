using AutoMapper;
using MediatR;
using MyMessenger.Domain.Entities;
using MyMessenger.Domain.Interfaces;
using MyMessenger.Application.DTO;
using MyMessenger.Application.DTO.UserDTOs;

namespace MyMessenger.Application.СommandsQueries.Users.Queries
{
    public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, DataForGridDTO<UserDTO>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        public GetAllUsersQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }
        public async Task<DataForGridDTO<UserDTO>> Handle(GetAllUsersQuery query, CancellationToken cancellationToken)
        {
            int pageNumber = query.PageNumber ?? 1;
            int pageSize = query.PageSize ?? 10;
            int skipSize = (pageNumber - 1) * pageSize;
            string subs = query.Subs ?? "";

            var dbQuery = unitOfWork.GetRepository<User>().GetAll();
            var queryResult = await unitOfWork.GetRepository<User>().FilterByQuery(dbQuery, query.Sort, skipSize, pageSize, subs);
            var resultMapped = mapper.Map<IEnumerable<UserDTO>>(queryResult);

            var numAllPages = unitOfWork.GetRepository<User>().GetNumberOfRecords();
            var numPages = (int)Math.Ceiling((double)numAllPages / pageSize);

            DataForGridDTO <UserDTO> result = new DataForGridDTO<UserDTO>() { Data = resultMapped, NumberOfPages = numPages };
            return result;
        }
    }
}
