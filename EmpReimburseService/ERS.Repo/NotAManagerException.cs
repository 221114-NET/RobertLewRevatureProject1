using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERS.Repo
{
    public class NotAManagerException : Exception
    {
        public NotAManagerException()
        {
        }

        public NotAManagerException(string? message) : base(message)
        {
        }

        public NotAManagerException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}