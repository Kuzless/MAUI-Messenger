using MediatR;
using MyMessenger.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyMessenger.Application.СommandsQueries.Chats.Commands
{
    public class JoinChatCommand : IRequest
    {
        public int Id { get; set; }
        public User User { get; set; }
        public JoinChatCommand(User User, int Id)
        {
            this.User = User;
            this.Id = Id;
        }
    }
}
