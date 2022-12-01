using System.Text.RegularExpressions;
using ERS.Model;

namespace ERS.Business
{
    public class BusLayer
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