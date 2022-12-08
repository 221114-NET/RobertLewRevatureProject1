using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ERS.Model;

namespace ERS.Repo
{
    public interface IRepository
    {
        Employee Register(Employee e);
    }

    public class Repository
    {
        public static Employee Register (Employee e)
        {
            return e;
        }
    }
}