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
        Employee Register(Employee e);
    }

    public class BusinessLogic
    {
        private readonly IRepository _repo;

        public BusinessLogic(IRepository repo)
        {
            _repo = repo;
        }

        public Employee Register(Employee e)
        {
            Regex validateEmailRegex = new Regex("^\\S+@(?:revature.net|revature.com)$");
            Regex validatePwRegex = new Regex("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{10,}$");

            if (!validateEmailRegex.IsMatch(e.Email))
            {
                return e;
            }
            else if (!validatePwRegex.IsMatch(e.Password))
            {
                return e;
            }

            return e;
        }

    }
}