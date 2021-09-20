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
    [Route("[Controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private BarBeerContext dbContext;
        public UserController(BarBeerContext context)
        {
            dbContext = context;
        }


        [HttpGet]
        public JsonResult Get()
        {
            return new JsonResult(dbContext.Users);
        }


        [HttpGet("{id}")]
        public async Task<JsonResult> Get(int id)
        {
            JsonResult result;
            User user = await dbContext.Users.FirstOrDefaultAsync(user => user.Id == id);
            if (user == null)
            {
                HttpContext.Response.StatusCode = 404;
                result = new JsonResult(StatusCode(404));
            }
            else
            {
                result = new JsonResult(user);
            }
            return result;
        }


        [HttpPost]
        public async Task<JsonResult> Post([FromBody]UserViewModel model)
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
            return new JsonResult(user.Id);
        }


        [HttpPut("{id}")]
        public async Task<JsonResult> Put(int id, [FromBody]UserViewModel model)
        {
            if (model == null)
            {
                HttpContext.Response.StatusCode = 404;
            }
            else
            {
                User user = await dbContext.Users.FirstOrDefaultAsync(user => user.Id == id);
                if (user == null)
                {
                    HttpContext.Response.StatusCode = 404;
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
                        HttpContext.Response.StatusCode = 200;
                    }
                    catch (Exception ex)
                    {
                        HttpContext.Response.StatusCode = 500;
                    }
                }
            }
            return new JsonResult(id);
        }

        [HttpDelete("{id}")]
        public async Task<JsonResult> Delete(int id)
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
                    dbContext.Users.Remove(user);
                    dbContext.SaveChanges();
                    HttpContext.Response.StatusCode = 200;
                }
                catch(Exception ex)
                {
                    HttpContext.Response.StatusCode = 500;
                }
            }
            return new JsonResult(id);
        }
    }
}
