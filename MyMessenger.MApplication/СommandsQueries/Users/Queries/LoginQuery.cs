using MediatR;
using MyMessenger.MApplication.DTO.AuthDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
