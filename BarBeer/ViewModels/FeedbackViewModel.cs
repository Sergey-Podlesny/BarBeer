using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BarBeer.ViewModels
{
    public class FeedbackViewModel
    {
        public int UserId { get; set; }
        public int BarId { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; }
    }
}
