using System;

namespace HueSharp.Exceptions
{
    public class ResourceDoesNotExistException : Exception
    {
        public ResourceDoesNotExistException(string message) : base(message)
        {

        }
    }
}
