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
        [Route("get-comments-by-bar-id")]
        public async Task<JsonResult> GetCommentsByBarId(int id)
        {
            var comments = await _barService.GetCommentsByBarIdAsync(id);
            return new JsonResult(comments);
        }

        [HttpGet]
        [Route("get-comments-by-user-id")]
        public async Task<JsonResult> GetCommentsByUserId(int id)
        {
            var comments = await _barService.GetCommentsByUserIdAsync(id);
            return new JsonResult(comments);
        }

        [HttpGet]
        [Route("get-bar-by-name")]
        public async Task<JsonResult> GetBarByName(string name)
        {
            var bar = await _barService.GetBarByNameAsync(name);
            return new JsonResult(bar);
        }

        [HttpGet]
        [Route("get-bars-by-rating")]
        public async Task<JsonResult> GetBarsByRating(double from = 0, double to = 5)
        {
            var bars = await _barService.GetBarsByRatingAsync(from, to);
            return new JsonResult(bars);
        }

        [HttpGet]
        [Route("get-bestbars-by-user-id")]
        public async Task<JsonResult> GetBestBarsByUserId(int id)
        {
            var bars = await _barService.GetPersonalBestBarsByUserId(id);
            return new JsonResult(bars);
        }

        
    }
}
