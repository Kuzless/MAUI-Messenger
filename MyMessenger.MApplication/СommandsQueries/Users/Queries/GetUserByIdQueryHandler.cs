using MediatR;
using MyMessenger.Domain.Entities;
using MyMessenger.MApplication.Services.Interfaces;

namespace MyMessenger.MApplication.СommandsQueries.Users.Queries
{
    public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, User>
    {
        private readonly IUserService userService;
        public GetUserByIdQueryHandler(IUserService userService)
        {
            this.userService = userService;
        }
        public async Task<User> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            return await userService.GetUserById(request.UserId);
        }
    }
}
