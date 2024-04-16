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
            int skipSize = (query.PageNumber - 1) * query.PageSize;

            var dbQuery = unitOfWork.GetRepository<User>().GetAll();
            var queryResult = await unitOfWork.GetRepository<User>().FilterByQuery(dbQuery, query.Sort, skipSize, query.PageSize, query.Subs);
            var resultMapped = mapper.Map<IEnumerable<UserDTO>>(queryResult.Keys.First());

            var numPages = (int)Math.Ceiling((double)queryResult.Values.First() / query.PageSize);

            DataForGridDTO <UserDTO> result = new DataForGridDTO<UserDTO>() { Data = resultMapped, NumberOfPages = numPages };
            return result;
        }
    }
}
