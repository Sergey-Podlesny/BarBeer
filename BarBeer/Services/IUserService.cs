using BarBeer.Models;
using BarBeer.ViewModels;
using BarBeer.ViewModels.ResponseViewModel;
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
        Task<User> GetUserByLogin(string login);
        Task<int> CreateUser(UserViewModel model);
        Task DeleteUserById(int id);
        Task UpdateUser(int id, UserViewModel model);
        Task<AuthViewModel> Authorization(UserViewModel model);
    }
}
