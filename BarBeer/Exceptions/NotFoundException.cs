using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BarBeer.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException() : base("Element not founded.") { }
        public NotFoundException(string message) : base(message) { }
    }
}
