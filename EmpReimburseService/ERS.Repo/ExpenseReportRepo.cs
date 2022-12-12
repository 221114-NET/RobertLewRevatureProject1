using Microsoft.Extensions.Logging;
using System.Data.SqlClient;
using ERS.Model;

namespace ERS.Repo
{
    public interface IExpenseReportRepo
    {
        Task<ExpenseReport> CreateReport(Employee emp);
        Task<Queue<ExpenseReport>> GetReports(Employee emp, string filter);
    }

    public class ExpenseReportRepo : IExpenseReportRepo
    {
        private readonly ILogger<EmployeeRepo> _logger;

        public ExpenseReportRepo(ILogger<EmployeeRepo> logger)
        {
            _logger = logger;
        }

        public async Task<ExpenseReport> CreateReport(Employee emp)
        {
            // user ADO.NET to push data to the DB.
            SqlConnection connection = new(@"Server=tcp:robertlew-revature.database.windows.net,1433;
                Initial Catalog=P1;Persist Security Info=False;
                User ID=robRevature;Password=Password1!;
                MultipleActiveResultSets=False;Encrypt=True;
                TrustServerCertificate=False;Connection Timeout=30;");
                
            await connection.OpenAsync();

            // set up query text
            string empCommandText;
            empCommandText = string.Format($"SELECT Employees.Id FROM dbo.Employees WHERE Employees.Email={emp.Email} AND Employees.Password={emp.Password}");

            using SqlCommand empCommand = new(empCommandText, connection);
            using SqlDataReader reader = await empCommand.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                emp.Id = reader.GetGuid(0);
            }
            ExpenseReport exp = new ExpenseReport();
            exp.Creator = emp.Id;

            string repCommandText;
            repCommandText = string.Format("INSERT INTO dbo.ExpenseReports(Creator, Type, Amount, Description) VALUES (@Creator, @Type, @Amount, @Description)");

            // configure the SQL query along with the connection object
            using SqlCommand command = new(repCommandText, connection);

            command.Parameters.AddWithValue("@Creator", exp.Creator);
            command.Parameters.AddWithValue("@Type", exp.Type);
            command.Parameters.AddWithValue("@Amount", exp.Amount);
            command.Parameters.AddWithValue("@Description", exp.Description);

            try { await command.ExecuteNonQueryAsync(); } catch (Exception ex) { _logger.LogError(ex, ex.Message); }

            _logger.LogInformation($"Added {exp.Type} {exp.Amount} as an expense to the DB");

            connection.Close();

            return exp;
        }

        public async Task<Queue<ExpenseReport>> GetReports(Employee emp, string filter)
        {
            Queue<ExpenseReport> reports = new Queue<ExpenseReport>();

            // user ADO.NET to push data to the DB.
            SqlConnection connection = new(@"Server=tcp:robertlew-revature.database.windows.net,1433;
                Initial Catalog=P1;Persist Security Info=False;
                User ID=robRevature;Password=Password1!;
                MultipleActiveResultSets=False;Encrypt=True;
                TrustServerCertificate=False;Connection Timeout=30;");

            await connection.OpenAsync();

            string commandText;

            if (emp.IsManager) commandText = string.Format($"SELECT * FROM dbo.ExpenseReports");
            else commandText = string.Format($"SELECT * FROM dbo.ExpenseReports WHERE Status = '{filter}'");

            using SqlCommand command = new(commandText, connection);
            using SqlDataReader reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                Guid id = reader.GetGuid(0);
                Guid creator = reader.GetGuid(1);
                string type = reader.GetString(2);
                double amount = reader.GetDouble(3);
                string description = reader.GetString(4);
                string status = reader.GetString(5);

                ExpenseReport tempReport = new ExpenseReport(id, creator, type, amount, description, Enum.Parse<TicketStatus>(status));
                reports.Enqueue(tempReport);
            }

            await connection.CloseAsync();

            _logger.LogInformation($"Executed GetReports, returned {reports.Count} results");

            return reports;
        }
    }
}