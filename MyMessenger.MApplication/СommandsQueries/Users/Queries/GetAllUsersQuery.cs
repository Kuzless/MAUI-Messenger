
using MediatR;
using MyMessenger.Domain.Entities;
using MyMessenger.MApplication.DTO;
using MyMessenger.MApplication.DTO.UserDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MyMessenger.MApplication.СommandsQueries.Users.Queries
{
    public class GetAllUsersQuery : IRequest<DataForGridDTO<UserDTO>>
    {
        public Dictionary<string, bool>? Sort { get; set; }
        public int? PageNumber { get; set; }
        public int? PageSize { get; set; }
        public string? Subs { get; set; }
        public GetAllUsersQuery(Dictionary<string, bool>? Sort, int? PageNumber, int? PageSize, string? Subs)
        {
            this.PageNumber = PageNumber;
            this.Sort = Sort;
            this.Subs = Subs;
            this.PageSize = PageSize;
        }
    }
}
