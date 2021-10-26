using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BarBeer.Exceptions
{
    public class InternalServerErrorException : Exception
    {
        public InternalServerErrorException() : base("Internal server error.") { }
        public InternalServerErrorException(string message) : base(message) { }
    }
}
