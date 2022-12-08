using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ERS.Model;
using ERS.Repo;

namespace Tests.Business
{
    public class MockRepository : IRepository
    {
        public Employee Register(Employee e)
        {
            return e;
        }
    }
}