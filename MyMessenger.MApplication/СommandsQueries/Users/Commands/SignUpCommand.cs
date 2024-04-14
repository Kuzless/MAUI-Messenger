using MediatR;
using MyMessenger.MApplication.DTO.AuthDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

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
