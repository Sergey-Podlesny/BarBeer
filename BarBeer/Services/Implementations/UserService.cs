using BarBeer.Context;
using BarBeer.Exceptions;
using BarBeer.Models;
using BarBeer.ViewModels;
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

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

        public async Task<int> CreateUser(UserViewModel model)
        {
            var user = new User 
            { 
                UserLogin = model.UserLogin, 
                UserPassword = model.UserPassword, 
                UserRole = model.UserRole 
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
    }
}
