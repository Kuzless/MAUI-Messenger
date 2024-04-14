using MediatR;
using MyMessenger.MApplication.DTO;
using MyMessenger.MApplication.DTO.AuthDTOs;

namespace MyMessenger.MApplication.СommandsQueries.Users.Commands
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
