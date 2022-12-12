using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using ERS.Model;
using ERS.Repo;
using Microsoft.Extensions.Logging;

namespace ERS.Logic
{
    public interface IEmployeeLogic
    {
        Task<Employee> Register(Employee e);
        Task<Employee> Login(string email, string pw);
        Task<Employee> UpdateInfo(string email, string pw, string fName = "", string lName = "", string newEmail = "", string newPw = "");
    }

    public class EmployeeLogic : IEmployeeLogic
    {
        private readonly IEmployeeRepo _repo;
        // private readonly ILogger<EmployeeLogic> _logger;

        public EmployeeLogic(IEmployeeRepo repo)
        {
            _repo = repo;
        }

        public async Task<Employee> Register(Employee e)
        {
            Regex validateEmailRegex = new Regex("^\\S+@(?:revature.net|revature.com)$");
            // Regex validatePwRegex = new Regex("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{10,}$");

            Employee e1 = await _repo.Register(e);

            if (!validateEmailRegex.IsMatch(e.Email))
            {
                throw new InvalidEmailException("Invalid email");
            }
            /* else if (!validatePwRegex.IsMatch(e.Password))
            {
                throw new InvalidEmailException("Invalid password");
            } */

            return e1;
        }

        public async Task<Employee> Login(string email, string pw)
        {
            Employee e1 = await _repo.GetEmployee(email, pw);

            return e1;
        }

        public async Task<Employee> UpdateInfo(string email, string pw, string fName = null, string lName = null, string newEmail = null, string newPw = null)
        {
            Employee employee = await _repo.GetEmployee(email, pw);

            if (fName != null) employee.FName = fName;
            if (lName != null) employee.LName = lName;
            if (newEmail != null) employee.Email = newEmail;
            if (newPw != null) employee.Password = newPw;

            return await _repo.UpdateEmployeeInfo(employee);
        }
    }
}