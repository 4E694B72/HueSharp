using System;

namespace HueSharp.Exceptions
{
    public class PhysicalButtonNotPressedException : Exception
    {
        public PhysicalButtonNotPressedException() : base("Link button is not pressed")
        {

        }
    }
}
