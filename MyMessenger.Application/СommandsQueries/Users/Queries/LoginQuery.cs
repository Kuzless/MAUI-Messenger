using MediatR;
using MyMessenger.Application.DTO.AuthDTOs;

namespace MyMessenger.Application.СommandsQueries.Users.Queries
{
    public class LoginQuery : IRequest<TokensDTO>
    {
        public LoginDTO User { get; set; }
        public LoginQuery(LoginDTO user)
        {
            User = user;
        }
    }
}
