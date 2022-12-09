using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using ERS.Model;
using ERS.Repo;

namespace ERS.Logic
{
    public interface IBusinessLogic
    {
        Task<Employee> Register(Employee e);
        Task<Employee> Login(Employee e);
    }

    public class BusinessLogic
    {
        private readonly IRepository _repo;

        public BusinessLogic(IRepository repo)
        {
            _repo = repo;
        }

        public async Task<Employee> Register(Employee e)
        {
            Regex validateEmailRegex = new Regex("^\\S+@(?:revature.net|revature.com)$");
            Regex validatePwRegex = new Regex("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{10,}$");

            Employee e1 = await _repo.Register(e);

            if (!validateEmailRegex.IsMatch(e.Email))
            {
                throw new InvalidEmailException("Invalid email");
            }
            else if (!validatePwRegex.IsMatch(e.Password))
            {
                throw new InvalidEmailException("Invalid password");
            }

            return e1;
        }

        public async Task<Employee> Login(Employee e)
        {
            Employee e1 = await _repo.GetEmployee(e.Email);

            return e1;
        }

    }
}