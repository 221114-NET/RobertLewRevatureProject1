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
        public async Task<Employee> Register(Employee e)
        {
            if (GetListOfEmployees().Any(emp => emp.Email == e.Email)) throw new EmployeeAlreadyExistsException("Employee already exists");

            return e;
        }

        public async Task<Employee> GetEmployee(string email)
        {
            List<Employee> employeesList = GetListOfEmployees();

            if (!employeesList.Any(emp => emp.Email == email)) throw new EmployeeNotFoundException("No such employee exists");

            return employeesList.FirstOrDefault(emp => emp.Email == email)!;
        }

        internal List<Employee> GetListOfEmployees()
        {
            Employee e1 = new Employee("joe123@revature.net", "Password1!");
            Employee e2 = new Employee("john123@revature.net", "Password1!");
            Employee e3 = new Employee("jane123@revature.net", "Password1!");
            Employee e4 = new Employee("mary123@revature.net", "Password1!");
            Employee e5 = new Employee("sam123@revature.net", "Password1!");

            List<Employee> employees = new List<Employee>() { e1, e2, e3, e4, e5 };

            return employees;
        }
    }
}