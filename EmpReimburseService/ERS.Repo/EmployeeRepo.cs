using Microsoft.Extensions.Logging;
using System.Data.SqlClient;
using ERS.Model;

namespace ERS.Repo
{
    public interface IEmployeeRepo
    {
        Task<Employee> Register(Employee e);
        Task<Employee> GetEmployee(string email, string pw);
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
            SqlConnection connection = new(@"Server=tcp:robertlew-revature.database.windows.net,1433;
                Initial Catalog=P1;Persist Security Info=False;
                User ID=robRevature;Password=Password1!;
                MultipleActiveResultSets=False;Encrypt=True;
                TrustServerCertificate=False;Connection Timeout=30;");


            if (await GetEmployee(newEmployee.Email, newEmployee.Password) != null) throw new EmployeeAlreadyExistsException("Employee already exists");

            // set up query text
            string commandText;
            commandText = string.Format("INSERT INTO dbo.Employees(FName, LName, Email, Password) VALUES (@FName, @LName, @Email, @Password)");

            // configure the SQL query along with the connection object
            using SqlCommand command = new(commandText, connection);
            await connection.OpenAsync();

            command.Parameters.AddWithValue("@FName", newEmployee.FName);
            command.Parameters.AddWithValue("@LName", newEmployee.LName);
            command.Parameters.AddWithValue("@Email", newEmployee.Email);
            command.Parameters.AddWithValue("@Password", newEmployee.Password);

            try { await command.ExecuteNonQueryAsync(); } catch (Exception ex) { _logger.LogError(ex, ex.Message); }

            _logger.LogInformation($"Added {newEmployee.FName} {newEmployee.LName} as an employee to the DB");

            connection.Close();

            return newEmployee;
        }

        public async Task<Employee> GetEmployee(string email, string pw)
        {
            Employee employee = new();

            SqlConnection connection = new(@"Server=tcp:robertlew-revature.database.windows.net,1433;
                Initial Catalog=P1;Persist Security Info=False;
                User ID=robRevature;Password=Password1!;
                MultipleActiveResultSets=False;Encrypt=True;
                TrustServerCertificate=False;Connection Timeout=30;");

            await connection.OpenAsync();

            string commandText = string.Format($"SELECT Employees.Id FROM dbo.Employees WHERE Employees.Email={email} AND Employees.Password={pw}");

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