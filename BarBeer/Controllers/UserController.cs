using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BarBeer.Context;
using BarBeer.Models;
using BarBeer.ViewModels;
using BarBeer.Services;
using BarBeer.Exceptions;

namespace BarBeer.Controllers
{
    [Route("[Controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }


        [HttpGet]
        public JsonResult Get()
        {
            var users = _userService.GetUsers();
            return new JsonResult(users);
        }


        [HttpGet("{id}")]
        public async Task<JsonResult> Get(int id)
        {
            JsonResult result;
            var user = await _userService.GetUserById(id);

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
            int id = default;
            try
            {
                id = await _userService.CreateUser(model);
                HttpContext.Response.StatusCode = 201;
            }
            catch(DbUpdateException)
            {
                HttpContext.Response.StatusCode = 400;
            }
            catch
            {
                HttpContext.Response.StatusCode = 500;
            }
            return new JsonResult(id);
        }


        [HttpPut("{id}")]
        public async Task<JsonResult> Put(int id, [FromBody]UserViewModel model)
        {
            try
            {
                await _userService.UpdateUser(id, model);
                HttpContext.Response.StatusCode = 200;
            }
            catch (InvalidModelException)
            {
                HttpContext.Response.StatusCode = 400;
            }
            catch (NotFoundException)
            {
                HttpContext.Response.StatusCode = 404;
            }
            catch (InternalServerErrorException)
            {
                HttpContext.Response.StatusCode = 500;
            }

            return new JsonResult(id);
        }

        [HttpDelete("{id}")]
        public async Task<JsonResult> Delete(int id)
        {
            try
            {
                await _userService.DeleteUserById(id);
                HttpContext.Response.StatusCode = 200;
            }
            catch(NotFoundException)
            {
                HttpContext.Response.StatusCode = 404;
            }
            catch
            {
                HttpContext.Response.StatusCode = 500;
            }
            return new JsonResult(id);
        }
    }
}
