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
    public class ERSController : ControllerBase
    {
        private readonly IEmployeeLogic _empLogic;
        private readonly IExpenseReportLogic _expLogic;
        private readonly ILogger<ERSController> _logger;

        public ERSController(IEmployeeLogic empLogic, IExpenseReportLogic expLogic, ILogger<ERSController> logger)
        {
            _empLogic = empLogic;
            _expLogic = expLogic;
            _logger = logger;
        }

        [HttpPost("api/registerEmployee")]
        public async Task<ActionResult<Employee>> PostNewEmployee([FromQuery]Employee newEmployee)
        {
            try { await _empLogic.Register(newEmployee); }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Problem("Did not create employee");
            }

            return Ok();
        }

        [HttpGet("api/employees/{emp.id}")]
        public async Task<ActionResult<Employee>> GetEmployee(string email, string pw)
        {
            Employee emp;

            try { emp = await _empLogic.Login(email, pw); }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return Problem("Did not retrieve employee data");
            }

            return Ok(emp);
        }

        [HttpPost("api/employees/{emp.id}/UserInfo")]
        public async Task<ActionResult<Employee>> UpdateEmployee(string email, string pw, string? fName = null, string? lName = null, string? newEmail = null, string? newPw = null)
        {
            Employee emp;

            try { emp = await _empLogic.UpdateInfo(email, pw, fName, lName, newEmail, newPw); }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return Problem("Employee not updated");
            }

            return Ok(emp);
        }

        [HttpPost("api/employees/{Id}/Tickets/Create")]
        public async Task<ActionResult<ExpenseReport>> PostExpenseReport([FromQuery] string email, string pw, ExpenseReport exp)
        {
            try { exp = await _expLogic.CreateTicket(email, pw, exp); }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return Problem("Did not create expense report");
            }

            return Ok(exp);
        }

        [HttpGet("api/employees/{emp.Id}/Tickets")]
        public async Task<ActionResult<ExpenseReport>> GetExpenseReports([FromQuery] string email, string pw, string filter)
        {
            Queue<ExpenseReport> reports;

            try { reports = await _expLogic.GetReports(email, pw, filter); }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return Problem("Did not return reports");
            }

            return Ok(reports);
        }

        [HttpGet("api/employees/{emp.Id}/Tickets/Status")]
        public async Task<ActionResult<ExpenseReport>> ChangeExpenseReportStatus([FromQuery] string email, string pw, string status)
        {
            ExpenseReport exp;

            try { exp = await _expLogic.ChangeReportStatus(email, pw, status); }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return Problem("Ticket not found");
            }

            return Ok(exp);
        }
    }
}