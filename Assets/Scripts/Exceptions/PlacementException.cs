using System;

namespace Exceptions
{
    public class PlacementException : Exception
    {
        public PlacementException(string message) : base(message) {}
    }
}
