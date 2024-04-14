using MyMessenger.MApplication.DTO.AuthDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyMessenger.MApplication.Services.JwtAuth.Interfaces
{
    public interface IJWTGeneratorService
    {
        public TokensDTO GenerateToken(string email, string id);
    }
}
