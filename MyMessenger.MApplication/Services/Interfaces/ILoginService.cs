using MyMessenger.MApplication.DTO.AuthDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyMessenger.MApplication.Services.Interfaces
{
    public interface ILoginService
    {
        public Task<TokensDTO> LoggingIn(LoginDTO user);
        public Task<TokensDTO> RefreshTokens(TokensDTO tokens);
    }
}
