using BarBeer.Context;
using BarBeer.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BarBeer.ViewModels;
using System.Text;
using BarBeer.Services;
using BarBeer.Exceptions;

namespace BarBeer.Controllers
{
    [Route("[Controller]")]
    [ApiController]
    public class BarController : ControllerBase
    {
        private IBarService _barService;
        public BarController(IBarService barService)
        {
            _barService = barService;
        }


        [HttpGet]
        public JsonResult GetBar()
        {
            var bars = _barService.GetBarsAsync();
            return new JsonResult(bars);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<JsonResult> GetBar(int id)
        {
            JsonResult result;
            var bar = await _barService.GetBarByIdAsync(id);
            if(bar != null)
            {
                result = new JsonResult(bar);
            }
            else
            {
                HttpContext.Response.StatusCode = 500;
                result = new JsonResult(StatusCode(500));
            }
            return result;
        }

        [HttpPost]
        public async Task<JsonResult> PostBar([FromBody] BarViewModel model)
        {
            var id = -1;
            try
            {
                id = await _barService.CreateBarAsync(model);
                HttpContext.Response.StatusCode = 201;
            }
            catch (DbUpdateException)
            {
                HttpContext.Response.StatusCode = 500;
            }
            return new JsonResult(id);
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<JsonResult> PutBar(int id, [FromBody] BarViewModel model)
        {
            try
            {
                await _barService.UpdateBarAsync(id, model);
            }
            catch (InternalServerErrorException)
            {
                HttpContext.Response.StatusCode = 500;
            }
            return new JsonResult(id);
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<JsonResult> DeleteBar(int id)
        {
            try
            {
                await _barService.DeleteBarByIdAsync(id);
            }
            catch (InternalServerErrorException)
            {
                HttpContext.Response.StatusCode = 500;
            }
            return new JsonResult(id);
        }

        [HttpGet]
        [Route("get-comments-by-bar-name")]
        public async Task<JsonResult> GetCommentsByBarName(string name)
        {
            var comments = await _barService.GetCommentsByBarNameAsync(name);
            return new JsonResult(comments);
        }

        [HttpGet]
        [Route("get-comments-by-user-login")]
        public async Task<JsonResult> GetCommentsByUserLogin(string login)
        {
            var comments = await _barService.GetCommentsByUserLoginAsync(login);
            return new JsonResult(comments);
        }
      
        [HttpGet]
        [Route("get-bars-by-rating-and-name")]
        public async Task<JsonResult> GetBarsByRatingAndName(double from = 0, double to = 5, string name = default)
        {
            var bars = await _barService.GetBarsByRatingAndNameAsync(from, to, name);
            return new JsonResult(bars);
        }

        [HttpGet]
        [Route("get-bestbars-by-user-id")]
        public async Task<JsonResult> GetBestBarsByUserId(int id)
        {
            var bars = await _barService.GetPersonalBestBarsByUserIdAsync(id);
            return new JsonResult(bars);
        }


        [HttpPost]
        [Route("save-bestbar")]
        public async Task<JsonResult> PostSaveBestBar(int barId, int userId)
        {
            var id = await _barService.SaveBestBar(barId, userId);
            return new JsonResult(id);
        }

        [HttpPost]
        [Route("leave-feedback")]
        public async Task<JsonResult> PostLeaveFeedback(FeedbackViewModel model)
        {
            JsonResult result;
            try
            {
                var currentRating = await _barService.LeaveFeedbackAsync(model);
                result = new JsonResult(currentRating);
                HttpContext.Response.StatusCode = 201;
            }
            catch (InternalServerErrorException)
            {
                HttpContext.Response.StatusCode = 500;
                result = new JsonResult(StatusCode(500));
            }
            return result;
        }
    }
}
