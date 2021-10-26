using BarBeer.Models;
using BarBeer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BarBeer.Services
{
    public interface IUserService
    {
        IEnumerable<User> GetUsers();
        Task<User> GetUserById(int id);
        Task<int> CreateUser(UserViewModel model);
        Task DeleteUserById(int id);
        Task UpdateUser(int id, UserViewModel model);
    }
}
