using MyMessenger.Application.DTO;
using MyMessenger.Application.DTO.AuthDTOs;

namespace MyMessenger.Application.Services.Interfaces
{
    public interface ISignUpService
    {
        public Task<ResponseDTO> SignUp(SignUpDTO signUpDTO);
    }
}
