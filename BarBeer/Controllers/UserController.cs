using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BarBeer.Context;
using BarBeer.Models;
using BarBeer.ViewModels;

namespace BarBeer.Controllers
{
    public class UserController : Controller
    {
        private BarBeerContext dbContext;
        public UserController(BarBeerContext context)
        {
            dbContext = context;
        }
        public IActionResult Index()
        {
            return default;
        }

        [HttpGet]
        public async Task<JsonResult> List(int id)
        {
            if (id != 0)
            {
                User user = await dbContext.Users.FirstOrDefaultAsync(user => user.Id == id);
                if(user == null)
                {
                    HttpContext.Response.StatusCode = 404;
                    return new JsonResult(StatusCode(404));
                }
                JsonResult jsonResult = new JsonResult(user);
                return jsonResult;
            }
            else
            {
                return new JsonResult(dbContext.Users);
            }
        }

        [HttpPost]
        public async void Create([FromBody]UserViewModel model)
        {
            User user = new User { UserLogin = model.UserLogin, UserPassword = model.UserPassword, UserRole = model.UserRole };
            await dbContext.AddAsync(user);
            try
            {
                dbContext.SaveChanges();
                HttpContext.Response.StatusCode = 201;
            }
            catch(DbUpdateException ex)
            {
                HttpContext.Response.StatusCode = 400;
            }
            catch
            {
                HttpContext.Response.StatusCode = 500;
            }
        }

        [HttpDelete]
        public async Task Delete(int id)
        {
            User user = await dbContext.Users.FirstOrDefaultAsync(user => user.Id == id);
            if (user == null)
            {
                HttpContext.Response.StatusCode = 404;
            }
            else
            {
                
                try
                {
                    dbContext.Remove(user);
                    dbContext.SaveChanges();
                    HttpContext.Response.StatusCode = 200;
                }
                catch(Exception ex)
                {
                    HttpContext.Response.StatusCode = 500;
                }
            }
        }

        [HttpPost]
        public async Task Edit([FromBody]UserViewModel model)
        {
            if (model == null)
            {
                HttpContext.Response.StatusCode = 404;
            }
            else
            {
                User user = await dbContext.Users.FirstOrDefaultAsync(user => user.Id == model.Id);
                user.UserLogin = model.UserLogin;
                user.UserPassword = model.UserPassword;
                user.UserRole = model.UserRole;
                try
                {
                    dbContext.Users.Update(user);
                    dbContext.SaveChanges();
                    HttpContext.Response.StatusCode = 200;
                }
                catch(Exception ex)
                {
                    HttpContext.Response.StatusCode = 500;
                }
            }
        }
    }
}
