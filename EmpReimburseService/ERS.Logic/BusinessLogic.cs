using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using ERS.Model;

namespace ERS.Logic
{
    public class BusinessLogic
    {
        public static string Register(Employee e)
        {
            Regex validateEmailRegex = new Regex("^\\S+@(?:revature.net|revature.com)$");

            Regex validatePwRegex = new Regex("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{10,}$");

            if (!validateEmailRegex.IsMatch(e.Email))
            {
                return "Invalid email";
            }
            else if (!validatePwRegex.IsMatch(e.Password))
            {
                return "Invalid password";
            }

            return e.Email;
        }

    }
}