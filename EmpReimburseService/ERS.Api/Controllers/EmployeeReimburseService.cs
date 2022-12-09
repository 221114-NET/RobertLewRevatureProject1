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
        private readonly ILogger<EmployeeReimburseService> _logger;

        public EmployeeReimburseService(IBusinessLogic logic, ILogger<EmployeeReimburseService> logger)
        {
            _logic = logic;
            _logger = logger;
        }

        [HttpPost(Name = "api/registerEmployee")]
        public async Task<ActionResult<Employee>> PostNewEmployee([FromBody] Employee newEmployee)
        {
            try
            {
                await _logic.Register(newEmployee);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Problem();
            }

            return Ok();
        }

        [HttpGet(Name = "api/employee/{id}")]
        public async Task<ActionResult<Employee>> GetEmployee([FromBody] Employee employee)
        {
            try
            {
                employee = await _logic.Login(employee);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return Problem();
            }

            return employee;
        }
    }
}