using MyMessenger.MApplication.DTO.AuthDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyMessenger.MApplication.Services.Interfaces
{
    public interface ISignUpService
    {
        public Task<SignUpResponseDTO> SignUp(SignUpDTO signUpDTO);
    }
}
