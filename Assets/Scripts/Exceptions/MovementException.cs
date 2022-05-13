using System;

namespace Exceptions
{
    public class MovementException : Exception
    {
        public MovementException(string message) : base(message) {}
    }
}
