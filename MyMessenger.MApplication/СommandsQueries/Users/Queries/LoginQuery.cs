using MediatR;
using MyMessenger.MApplication.DTO.AuthDTOs;

namespace MyMessenger.MApplication.СommandsQueries.Users.Queries
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
