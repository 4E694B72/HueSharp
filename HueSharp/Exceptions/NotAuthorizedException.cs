using System;

namespace HueSharp.Exceptions
{
    public class NotAuthorizedException : Exception
    {
        public NotAuthorizedException() : base("Application key is not valid")
        {

        }
    }
}
