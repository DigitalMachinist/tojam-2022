using System;

namespace Exceptions
{
    public class SelectionException : Exception
    {
        public SelectionException(string message) : base(message) {}
    }
}
