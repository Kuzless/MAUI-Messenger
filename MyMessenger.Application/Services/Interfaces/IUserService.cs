using MyMessenger.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyMessenger.Application.Services.Interfaces
{
    public interface IUserService
    {
        Task<User> GetUserById(string id);
    }
}
