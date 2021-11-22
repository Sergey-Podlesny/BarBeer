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
using Microsoft.Data.SqlClient;
using System.Text;

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


        [HttpGet]
        [Route("{id}")]
        public async Task<JsonResult> Get(int id)
        {
            JsonResult result;
            var user = await _userService.GetUserById(id);
            if(user != null)
            {
                result = new JsonResult(user);
            }
            else
            {
                HttpContext.Response.StatusCode = 500;
                result = new JsonResult(StatusCode(500));
            }
            return result;
        }

        [HttpPost]
        [Route("authorization")]
        public async Task<JsonResult> PostAuthUser([FromBody] UserViewModel model)
        {

            var authViewModel = await _userService.Authorization(model);

            return new JsonResult(authViewModel);
        }


        [HttpPost]
        public async Task<JsonResult> PostCreateUser([FromBody]UserViewModel model)
        {
            int id = default;
            try
            {
                id = await _userService.CreateUser(model);
                HttpContext.Response.StatusCode = 201;
            }
            catch(InvalidModelException ex)
            {
                HttpContext.Response.StatusCode = 400;
                await Microsoft.AspNetCore.Http.HttpResponseWritingExtensions.WriteAsync(HttpContext.Response, ex.Message);
            }
            catch(DbUpdateException ex)
            {
                var sqlEx = ex.InnerException as SqlException;
                HttpContext.Response.StatusCode = 500;
                if(sqlEx.Number == 2627)
                {
                    await Microsoft.AspNetCore.Http.HttpResponseWritingExtensions.WriteAsync(HttpContext.Response, "Пользователь с таким логином уже существует.");
                }
            }

            return new JsonResult(id);
        }


        [HttpPut]
        [Route("{id}")]
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
            catch (InternalServerErrorException)
            {
                HttpContext.Response.StatusCode = 500;
            }

            return new JsonResult(id);
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<JsonResult> Delete(int id)
        {
            try
            {
                await _userService.DeleteUserById(id);
                HttpContext.Response.StatusCode = 200;
            }
            catch(InternalServerErrorException)
            {
                HttpContext.Response.StatusCode = 500;
            }
            return new JsonResult(id);
        }
    }
}
