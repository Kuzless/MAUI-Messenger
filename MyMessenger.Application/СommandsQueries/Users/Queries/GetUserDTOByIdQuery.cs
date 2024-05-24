
using MediatR;
using MyMessenger.Application.DTO.UserDTOs;

namespace MyMessenger.Application.СommandsQueries.Users.Queries
{
    public class GetUserDTOByIdQuery : IRequest<UserDTO>
    {
        public string UserId { get; set; }
        public GetUserDTOByIdQuery(string userId)
        {

            UserId = userId;

        }
    }
}
