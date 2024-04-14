using MediatR;
using MyMessenger.Application.DTO.AuthDTOs;

namespace MyMessenger.Application.СommandsQueries.Users.Queries
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
