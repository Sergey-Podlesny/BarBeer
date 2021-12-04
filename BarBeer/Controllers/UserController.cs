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
using BarBeer.ViewModels.ResponseViewModel;

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
            RegViewModel regViewModel = new RegViewModel();

            try
            {
                regViewModel.Id = await _userService.CreateUser(model);
                HttpContext.Response.StatusCode = 201;
            }
            catch(InvalidModelException ex)
            {
                regViewModel.Errors.Add(ex.Message);
            }
            catch(DbUpdateException ex)
            {
                var sqlEx = ex.InnerException as SqlException;
                if(sqlEx.Number == 2627)
                {
                    regViewModel.Errors.Add("Пользователь с таким логином уже существует.");
                }
            }

            return new JsonResult(regViewModel);
        }


        [HttpPut]
        [Route("{id}")]
        public async Task<JsonResult> Put(int id, [FromBody]UserViewModel model)
        {
            UpdViewModel updViewModel = new UpdViewModel() { Id = id };

            try
            {
                await _userService.UpdateUser(id, model);
            }
            catch (InvalidModelException ex)
            {
                updViewModel.Errors.Add(ex.Message);
            }
            catch (InternalServerErrorException ex)
            {
                updViewModel.Errors.Add(ex.Message);
            }

            return new JsonResult(updViewModel);
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<JsonResult> Delete(int id)
        {
            try
            {
                await _userService.DeleteUserById(id);
            }
            catch(InternalServerErrorException)
            {
                HttpContext.Response.StatusCode = 500;
            }
            return new JsonResult(id);
        }
    }
}
