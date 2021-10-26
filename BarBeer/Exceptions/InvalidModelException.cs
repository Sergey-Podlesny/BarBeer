using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BarBeer.Exceptions
{
    public class InvalidModelException : Exception
    {
        public InvalidModelException() : base("Invalid model.") { }
        public InvalidModelException(string message) : base(message) { }
    }
}
