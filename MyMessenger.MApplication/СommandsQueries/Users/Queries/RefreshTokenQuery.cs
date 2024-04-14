using MediatR;
using MyMessenger.MApplication.DTO.AuthDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
