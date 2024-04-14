using MediatR;
using MyMessenger.Application.DTO;
using MyMessenger.Application.DTO.AuthDTOs;

namespace MyMessenger.Application.СommandsQueries.Users.Commands
{
    public class SignUpCommand : IRequest<ResponseDTO>
    {
        public SignUpDTO User;
        public SignUpCommand(SignUpDTO User)
        {
            this.User = User;
        }
    }
}
