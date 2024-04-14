using MediatR;
using MyMessenger.MApplication.DTO.AuthDTOs;

namespace MyMessenger.MApplication.СommandsQueries.Users.Queries
{
    public class RefreshTokenQuery : IRequest<TokensDTO>
    {
        public TokensDTO Tokens { get; set; }
        public RefreshTokenQuery(TokensDTO tokens) 
        { 
            Tokens = tokens;
        }

    }
}
