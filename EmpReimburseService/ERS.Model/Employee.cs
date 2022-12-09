using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;

namespace ERS.Model
{
    public class Employee
    {
        public Guid Id { get; set; }
        public string FName { get; set; } = "";
        public string LName { get; set; } = "";
        public string Email { get; set; } = "";
        public string Password { get; set; } = "";
        public bool IsManager { get; set; } = false;

        public Employee() { }

        public Employee(string email, string password, string fName = "", string lName = "")
        {
            FName = fName;
            LName = lName;
            Email = email;
            Password = password;
        }

        public Employee(Guid id, string fName, string lName, string email, string password, bool isManager)
        {
            Id = id;
            FName = fName;
            LName = lName;
            Email = email;
            Password = password;
            IsManager = isManager;
        }
    }
}