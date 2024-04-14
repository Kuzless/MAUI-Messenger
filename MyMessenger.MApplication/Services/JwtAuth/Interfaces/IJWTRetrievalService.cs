using MyMessenger.MApplication.DTO.AuthDTOs;

namespace MyMessenger.MApplication.Services.JwtAuth.Interfaces
{
    public interface IJWTRetrievalService
    {
        string GetIdByToken(TokensDTO tokens);
        string GetEmailByToken(TokensDTO tokens);
    }
}
