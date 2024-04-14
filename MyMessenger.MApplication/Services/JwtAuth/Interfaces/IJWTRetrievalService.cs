using MyMessenger.MApplication.DTO.AuthDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyMessenger.MApplication.Services.JwtAuth.Interfaces
{
    public interface IJWTRetrievalService
    {
        string GetIdByToken(TokensDTO tokens);
        string GetEmailByToken(TokensDTO tokens);
    }
}
