using MediatR;
using MyMessenger.MApplication.DTO.AuthDTOs;

namespace MyMessenger.MApplication.СommandsQueries.Users.Commands
{
    public class SignUpCommand : IRequest<SignUpResponseDTO>
    {
        public SignUpDTO User;
        public SignUpCommand(SignUpDTO User)
        {
            this.User = User;
        }
    }
}
