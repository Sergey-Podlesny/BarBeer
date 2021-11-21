using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BarBeer.ViewModels
{
    public class AuthViewModel
    {
        public bool Status { get; set; }
        public List<string> Errors { get; set; }
    }
}
