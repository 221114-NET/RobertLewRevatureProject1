using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ERS.Model;
using ERS.Logic;

namespace ERS.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeeReimburseService : ControllerBase
    {
        private readonly IBusinessLogic _logic;

        public EmployeeReimburseService(IBusinessLogic logic)
        {
            _logic = logic;
        }

        [HttpPost()]
        public Employee Register(Employee e)
        {
            Employee e1 = _logic.Register(e);
            
            return e1;
        }
    }
}