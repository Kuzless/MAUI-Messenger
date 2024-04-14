using MyMessenger.MApplication.DTO;
using MyMessenger.MApplication.DTO.AuthDTOs;

namespace MyMessenger.MApplication.Services.Interfaces
{
    public interface ISignUpService
    {
        public Task<ResponseDTO> SignUp(SignUpDTO signUpDTO);
    }
}
