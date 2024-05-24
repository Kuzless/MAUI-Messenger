
using AutoMapper;
using MediatR;
using MyMessenger.Application.DTO.UserDTOs;
using MyMessenger.Application.Services.Interfaces;

namespace MyMessenger.Application.СommandsQueries.Users.Queries
{
    public class GetUserDTOByIdQueryHandler : IRequestHandler<GetUserDTOByIdQuery, UserDTO>
    {
        private readonly IUserService userService;
        private readonly IMapper mapper;
        public GetUserDTOByIdQueryHandler(IUserService userService, IMapper mapper)
        {
            this.userService = userService;
            this.mapper = mapper;
        }

        public async Task<UserDTO> Handle(GetUserDTOByIdQuery request, CancellationToken cancellationToken)
        {
            var user = await userService.GetUserById(request.UserId);
            return mapper.Map<UserDTO>(user);
        }
    }
}
