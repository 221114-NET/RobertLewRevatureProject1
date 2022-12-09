using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERS.Repo
{
    public class InvalidPwException : Exception
    {
        public InvalidPwException()
        {
        }

        public InvalidPwException(string? message) : base(message)
        {
        }

        public InvalidPwException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}