using BarBeer.Context;
using BarBeer.Exceptions;
using BarBeer.Models;
using BarBeer.ViewModels;
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Mail;
using BarBeer.ViewModels.ResponseViewModel;

namespace BarBeer.Services.Implementations
{
    public class UserService : IUserService
    {
        private BarBeerContext dbContext;
        public UserService(BarBeerContext context)
        {
            dbContext = context;
        }

        public IEnumerable<User> GetUsers()
        {
            return dbContext.Users.AsEnumerable();
        }
        public async Task<User> GetUserById(int id)
        {
            return await dbContext.Users.FirstOrDefaultAsync(user => user.Id == id);
        }

        public async Task<User> GetUserByLogin(string login)
        {
            return await dbContext.Users.FirstOrDefaultAsync(user => user.UserLogin == login);
        }

        public async Task<int> CreateUser(UserViewModel model)
        {

            model.CheckValid();

            var user = new User 
            { 
                UserLogin = model.UserLogin, 
                UserPassword = model.UserPassword, 
                UserRole = model.UserRole,
                UserEmail = model.UserEmail
            };

            await dbContext.Users.AddAsync(user);
            await dbContext.SaveChangesAsync();
            return user.Id;
        }
        public async Task DeleteUserById(int id)
        {
            var user = await dbContext.Users.FirstOrDefaultAsync(user => user.Id == id);
            if (user == null)
            {
                throw new InternalServerErrorException();
            }
            else
            {
                dbContext.Users.Remove(user);
                await dbContext.SaveChangesAsync();
            }
        }
        public async Task UpdateUser(int id, UserViewModel model)
        {
            model.CheckValid();

            var user = await dbContext.Users.FirstOrDefaultAsync(user => user.Id == id);
            if (user == null)
            {
                throw new InternalServerErrorException("Пользователя с таким ID не существует.");
            }
            else
            {
                user.UserLogin = model.UserLogin;
                user.UserPassword = model.UserPassword;
                user.UserRole = model.UserRole;
                user.UserEmail = model.UserEmail;
                dbContext.Users.Update(user);
                dbContext.SaveChanges();
            }
        }

        public async Task<AuthViewModel> Authorization(UserViewModel model)
        {
            AuthViewModel authViewModel = new AuthViewModel()
            {
                Status = true,
                Errors = new List<string>()
            };

            var user = await GetUserByLogin(model.UserLogin);
            if (user == null)
            {
                authViewModel.Errors.Add("Неверный логин или пароль.");
                authViewModel.Status = false;
            }
            else if (user.UserPassword != model.UserPassword)
            {
                authViewModel.Errors.Add("Неверный логин или пароль.");
                authViewModel.Status = false;
            }

            return authViewModel;
        }
    }
}
