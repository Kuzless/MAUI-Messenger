using MyMessenger.MApplication.DTO.AuthDTOs;

namespace MyMessenger.MApplication.Services.Interfaces
{
    public interface ISignUpService
    {
        public Task<SignUpResponseDTO> SignUp(SignUpDTO signUpDTO);
    }
}
