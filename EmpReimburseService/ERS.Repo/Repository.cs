using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System.Data.SqlClient;
using ERS.Model;

namespace ERS.Repo
{
    public interface IRepository
    {
        Task<Employee> Register(Employee e);
        Task<Employee> GetEmployee(string email);
    }

    public class Repository : IRepository
    {
        public readonly string _connectionString;
        private readonly ILogger<Repository> _logger;

        public Repository(string connectionString, ILogger<Repository> logger)
        {
            _connectionString = connectionString;
            _logger = logger;
        }

        public async Task<Employee> Register(Employee newEmployee)
        {
            // user ADO.NET to push data to the DB.
            SqlConnection connection = new(_connectionString);
            await connection.OpenAsync();

            if (await GetEmployee(newEmployee.Email) != null) throw new EmployeeAlreadyExistsException("Employee already exists");

            // set up query text
            string commandText;
            commandText = string.Format("INSTER INTO dbo.Employees(Fname, Lname, Email, Password) VALUES (@FName, @LName, @Email, @Password)");

            // configure the SQL query along with the connection object
            using SqlCommand command = new(commandText, connection);

            command.Parameters.AddWithValue("@FName", newEmployee.FName);
            command.Parameters.AddWithValue("@LName", newEmployee.LName);
            command.Parameters.AddWithValue("@Email", newEmployee.Email);
            command.Parameters.AddWithValue("@Password", newEmployee.Password);

            try { await command.ExecuteNonQueryAsync(); } catch (Exception ex) { _logger.LogError(ex, ex.Message); }

            _logger.LogInformation($"Added {newEmployee.FName} {newEmployee.LName} as an employee to the DB");

            connection.Close();

            return newEmployee;
        }

        public async Task<Employee> GetEmployee(string email)
        {
            Employee employee = new();

            using SqlConnection connection = new(_connectionString);
            await connection.OpenAsync();

            string commandText = string.Format($"SELECT Employees.Id FROM dbo.Employees WHERE Employees.Email={email}");

            // Getting employee Id
            using SqlCommand command = new(commandText, connection);
            using SqlDataReader reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                Guid id = reader.GetGuid(0);
                string fName = reader.GetString(1);
                string lName = reader.GetString(2);
                string tempEmail = reader.GetString(3);
                string password = reader.GetString(4);
                bool isManager = reader.GetBoolean(5);

                Employee tempEmp = new Employee(id, fName, lName, tempEmail, password, isManager);
                employee = tempEmp;
            }

            await connection.CloseAsync();

            _logger.LogInformation("Executed GetEmployee");

            return employee;
        }
    }
}