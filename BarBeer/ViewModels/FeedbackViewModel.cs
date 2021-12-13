using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BarBeer.ViewModels
{
    public class FeedbackViewModel
    {
        public string login { get; set; }
        public string barName { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; }
    }
}
