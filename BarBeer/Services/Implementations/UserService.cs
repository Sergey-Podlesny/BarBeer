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
            var user = await dbContext.Users.FirstOrDefaultAsync(user => user.Id == id);
            if(user == null)
            {
                throw new NotFoundException();
            }
            return user;
        }

        public async Task<User> GetUserByLogin(string login)
        {
            var user = await dbContext.Users.FirstOrDefaultAsync(user => user.UserLogin == login);
            if (user == null)
            {
                throw new NotFoundException();
            }
            return user;
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

            await dbContext.AddAsync(user);
            await dbContext.SaveChangesAsync();
            return user.Id;
        }
        public async Task DeleteUserById(int id)
        {
            var user = await dbContext.Users.FirstOrDefaultAsync(user => user.Id == id);
            if (user == null)
            {
                throw new NotFoundException();
            }
            else
            {
                dbContext.Users.Remove(user);
                await dbContext.SaveChangesAsync();
            }
        }
        public async Task UpdateUser(int id, UserViewModel model)
        {
            if (model == null)
            {
                throw new InvalidModelException();
            }
            else
            {
                var user = await dbContext.Users.FirstOrDefaultAsync(user => user.Id == id);
                if (user == null)
                {
                    throw new NotFoundException();
                }
                else
                {
                    user.UserLogin = model.UserLogin;
                    user.UserPassword = model.UserPassword;
                    user.UserRole = model.UserRole;
                    try
                    {
                        dbContext.Users.Update(user);
                        dbContext.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        throw new InternalServerErrorException(ex.Message);
                    }
                }
            }
        }

        public async Task<AuthViewModel> Authorization(UserViewModel model)
        {
            AuthViewModel authViewModel = new AuthViewModel()
            {
                Status = true,
                Errors = new List<string>()
            };

            try
            {
                var user = await GetUserByLogin(model.UserLogin);
                if (user.UserPassword != model.UserPassword)
                {
                    authViewModel.Errors.Add("Неверный пароль.");
                    authViewModel.Status = false;
                }
            }
            catch(NotFoundException)
            {
                authViewModel.Errors.Add("Неверный логин.");
                authViewModel.Status = false;
            }

            return authViewModel;
        }
    }
}
