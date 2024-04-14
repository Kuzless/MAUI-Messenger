using MediatR;
using MyMessenger.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyMessenger.MApplication.СommandsQueries.Users.Queries
{
    public class GetUserByIdQuery : IRequest<User>
    {
        public string UserId { get; set; }
        public GetUserByIdQuery(string userId)
        {

            UserId = userId;

        }
    }
}
