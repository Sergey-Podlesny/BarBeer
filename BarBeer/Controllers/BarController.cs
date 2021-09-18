using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BarBeer.Controllers
{
    public class BarController : Controller
    {

        public JsonResult BarList(int id)
        {
            if(id == 0)
            {
                return new JsonResult("no id");
            }
            else
            {
                return ConcreteBar(id);
            }
        }

        private JsonResult ConcreteBar(int id)
        {
            return new JsonResult(id);
        }
    }
}
