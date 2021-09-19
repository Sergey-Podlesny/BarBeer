using BarBeer.Context;
using BarBeer.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace BarBeer.Controllers
{
    public class BarController : Controller
    {
        private BarBeerContext dbContext;
        public BarController(BarBeerContext context)
        {
            dbContext = context;
        }

        public JsonResult Index()
        {
            return default;
        }
        public async Task<JsonResult> List(int id)
        {
            if (id != 0)
            {
                Bar bar = await dbContext.Bars.FirstOrDefaultAsync(bar => bar.Id == id);
                JsonResult jsonResult = new JsonResult(bar);
                return jsonResult;
            }
            else
            {
                return new JsonResult(dbContext.Bars);
            }
        }

        
    }
}
