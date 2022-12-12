using Microsoft.Extensions.Logging;
using System.Data.SqlClient;
using ERS.Model;

namespace ERS.Repo
{
    public interface IEmployeeRepo
    {
        Task<Employee> Register(Employee e);
        Task<Employee> GetEmployee(string email, string pw);
        Task<Employee> UpdateEmployeeInfo(Employee emp);
    }

    public class EmployeeRepo : IEmployeeRepo
    {
        private readonly ILogger<EmployeeRepo> _logger;

        public EmployeeRepo(ILogger<EmployeeRepo> logger)
        {
            _logger = logger;
        }

        public async Task<Employee> Register(Employee newEmployee)
        {
            // user ADO.NET to push data to the DB.
            SqlConnection connection = new SqlConnection(@"Connection String");


            // if (await GetEmployee(newEmployee.Email, newEmployee.Password) != null) throw new EmployeeAlreadyExistsException("Employee already exists");

            // set up query text
            string commandText;
            commandText = string.Format("INSERT INTO Employees(FName, LName, Email, Password) VALUES (@fName, @lName, @email, @password);");

            // configure the SQL query along with the connection object
            using SqlCommand command = new(commandText, connection);
            await connection.OpenAsync();

            command.Parameters.AddWithValue("@fName", newEmployee.FName);
            command.Parameters.AddWithValue("@lName", newEmployee.LName);
            command.Parameters.AddWithValue("@email", newEmployee.Email);
            command.Parameters.AddWithValue("@password", newEmployee.Password);

            try { await command.ExecuteNonQueryAsync(); } catch (Exception ex) { _logger.LogError(ex, ex.Message); }

            _logger.LogInformation($"Added {newEmployee.FName} {newEmployee.LName} as an employee to the DB");

            connection.Close();
            return newEmployee;
        }

        public async Task<Employee> GetEmployee(string email, string pw)
        {
            Employee employee = new();

            SqlConnection connection = new SqlConnection(@"Connection string");

            await connection.OpenAsync();

            string commandText = string.Format($"SELECT * FROM dbo.Employees WHERE Employees.Email='{email}' AND Employees.Password='{pw}'");

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

        public async Task<Employee> UpdateEmployeeInfo(Employee emp)
        {
            // user ADO.NET to push data to the DB.
            SqlConnection connection = new SqlConnection(@"Connection String");

            await connection.OpenAsync();

            string commandText;
            commandText = string.Format($"UPDATE Employees SET FName = '{emp.FName}', LName = '{emp.LName}', Email = '{emp.Email}', Password = '{emp.Password}' WHERE Id = '{emp.Id}'");
            using SqlCommand command = new(commandText, connection);

            try { await command.ExecuteNonQueryAsync(); } catch (Exception ex) { _logger.LogError(ex, ex.Message); }
            await connection.CloseAsync();

            _logger.LogInformation($"Employee info has been updated");

            return emp;
        }
    }
}