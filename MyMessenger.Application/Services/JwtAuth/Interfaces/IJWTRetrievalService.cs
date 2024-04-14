using MyMessenger.Application.DTO.AuthDTOs;

namespace MyMessenger.Application.Services.JwtAuth.Interfaces
{
    public interface IJWTRetrievalService
    {
        string GetIdByToken(TokensDTO tokens);
        string GetEmailByToken(TokensDTO tokens);
    }
}
