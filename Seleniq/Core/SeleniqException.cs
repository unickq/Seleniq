using System;

namespace Seleniq.Core
{
    public class SeleniqException : Exception
    {
        public SeleniqException(string message) : base(message)
        {
        }
    }
}
