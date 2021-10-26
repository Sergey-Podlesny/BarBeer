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
            var bars = _barService.GetBars();
            return new JsonResult(bars);
        }

        [HttpGet("{id}")]
        public async Task<JsonResult> GetBar(int id)
        {
            JsonResult result;
            var bar = await _barService.GetBarById(id);
            
            if (bar == null)
            {
                HttpContext.Response.StatusCode = 404;
                result = new JsonResult(StatusCode(404));
            }
            else
            {
                result = new JsonResult(bar);
            }
            return result;

        }


        [HttpPost]
        public async Task<JsonResult> PostBar([FromBody] BarViewModel model)
        {
            int id = default;
            try
            {
                id = await _barService.CreateBar(model);
                HttpContext.Response.StatusCode = 201;
            }
            catch (DbUpdateException)
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
        public async Task<JsonResult> PutBar(int id, [FromBody] BarViewModel model)
        {
            try
            {
                await _barService.UpdateBar(id, model);
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
        public async Task<JsonResult> DeleteBar(int id)
        {
            try
            {
                await _barService.DeleteBarById(id);
                HttpContext.Response.StatusCode = 200;
            }
            catch (NotFoundException)
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
