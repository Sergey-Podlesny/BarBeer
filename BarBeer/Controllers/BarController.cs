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

namespace BarBeer.Controllers
{
    [Route("[Controller]")]
    [ApiController]
    public class BarController : ControllerBase
    {
        private BarBeerContext dbContext;
        public BarController(BarBeerContext context)
        {
            dbContext = context;
        }


        [HttpGet]
        public JsonResult Get()
        {
            return new JsonResult(dbContext.Bars);
        }

        [HttpGet("{id}")]
        public async Task<JsonResult> Get(int id)
        {
            JsonResult result;
            Bar bar = await dbContext.Bars.FirstOrDefaultAsync(bar => bar.Id == id);
            if (bar == null)
            {
                HttpContext.Response.StatusCode = 404;
                //byte[] data = Encoding.ASCII.GetBytes("Incorrected ID");
                //await HttpContext.Response.Body.WriteAsync(data, 0, data.Length);
                result = new JsonResult(StatusCode(404));
            }
            else
            {
                result = new JsonResult(bar);

            }
            return result;

        }


        [HttpPost]
        public async Task<JsonResult> Post([FromBody] BarViewModel model)
        {
            Bar bar = new Bar { BarName = model.BarName, BarImage = model.BarImage, BarRating = model.BarRating, BarLocation = model.BarLocation };
            await dbContext.AddAsync(bar);
            try
            {
                dbContext.SaveChanges();
                HttpContext.Response.StatusCode = 201;
            }
            catch (DbUpdateException ex)
            {
                HttpContext.Response.StatusCode = 400;
                //byte[] data = Encoding.ASCII.GetBytes(ex.Message);
                //await HttpContext.Response.Body.WriteAsync(data, 0, data.Length);
            }
            catch
            {
                HttpContext.Response.StatusCode = 500;
            }
            return new JsonResult(bar.Id);
        }

        [HttpPut("{id}")]
        public async Task<JsonResult> Put(int id, [FromBody] BarViewModel model)
        {
            if(model == null)
            {
                HttpContext.Response.StatusCode = 404;
            }
            else
            {
                Bar bar = await dbContext.Bars.FirstOrDefaultAsync(bar => bar.Id == id);
                if(bar == null)
                {
                    HttpContext.Response.StatusCode = 404;
                }
                else
                {
                    bar.BarName = model.BarName;
                    bar.BarImage = model.BarImage;
                    bar.BarRating = model.BarRating;
                    bar.BarLocation = model.BarLocation;
                    try
                    {
                        dbContext.Bars.Update(bar);
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
            Bar bar = await dbContext.Bars.FirstOrDefaultAsync(bar => bar.Id == id);
            if (bar == null)
            {
                HttpContext.Response.StatusCode = 404;
            }
            else
            {
                try
                {
                    dbContext.Bars.Remove(bar);
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
