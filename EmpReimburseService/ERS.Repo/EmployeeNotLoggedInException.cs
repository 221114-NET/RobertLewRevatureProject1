using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERS.Repo
{
    public class EmployeeNotLoggedInException : Exception
    {
        public EmployeeNotLoggedInException()
        {
        }

        public EmployeeNotLoggedInException(string? message) : base(message)
        {
        }

        public EmployeeNotLoggedInException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}