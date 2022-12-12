using Microsoft.Extensions.Logging;
using System.Data.SqlClient;
using ERS.Model;

namespace ERS.Repo
{
    public interface IExpenseReportRepo
    {
        Task<ExpenseReport> CreateReport(string email, string pw, ExpenseReport exp);
        Task<Queue<ExpenseReport>> GetReports(string email, string pw, string filter);
        void UpdateReport(ExpenseReport exp);
    }

    public class ExpenseReportRepo : IExpenseReportRepo
    {
        private readonly IEmployeeRepo _empRepo;
        private readonly ILogger<EmployeeRepo> _logger;

        public ExpenseReportRepo(IEmployeeRepo empRepo, ILogger<EmployeeRepo> logger)
        {
            _empRepo = empRepo;
            _logger = logger;
        }

        public async Task<ExpenseReport> CreateReport(string email, string pw, ExpenseReport exp)
        {
            Employee employee = await _empRepo.GetEmployee(email, pw);
            exp.Creator = employee.Id;

            // user ADO.NET to push data to the DB.
            SqlConnection connection = new SqlConnection(@"Connection String");

            await connection.OpenAsync();

            // set up query text
            string commandText;
            commandText = string.Format("INSERT INTO ExpenseReports(Creator, Type, Amount, Description) VALUES (@creator, @type, @amount, @description)");

            // configure the SQL query along with the connection object
            using SqlCommand command = new(commandText, connection);

            command.Parameters.AddWithValue("@creator", exp.Creator);
            command.Parameters.AddWithValue("@type", exp.Type);
            command.Parameters.AddWithValue("@amount", exp.Amount);
            command.Parameters.AddWithValue("@description", exp.Description);

            try { await command.ExecuteNonQueryAsync(); } catch (Exception ex) { _logger.LogError(ex, ex.Message); }

            _logger.LogInformation($"Added a {exp.Type} type of expense in the amount of: {exp.Amount} as an expense to the DB");

            connection.Close();

            return exp;
        }

        public async Task<Queue<ExpenseReport>> GetReports(string email, string pw, string filter)
        {
            Queue<ExpenseReport> reports = new Queue<ExpenseReport>();
            Employee emp = await _empRepo.GetEmployee(email, pw);

            // user ADO.NET to push data to the DB.
            SqlConnection connection = new SqlConnection(@"Connection String");

            await connection.OpenAsync();

            string commandText;

            if (emp.IsManager) commandText = string.Format($"SELECT * FROM dbo.ExpenseReports WHERE Status NOT IN ('{TicketStatus.Approved.ToString()}', '{TicketStatus.Rejected.ToString()}')");
            else commandText = string.Format($"SELECT * FROM dbo.ExpenseReports WHERE Status = '{filter}' AND Creator = '{emp.Id}'");

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

        public async void UpdateReport(ExpenseReport exp)
        {
            // user ADO.NET to push data to the DB.
            SqlConnection connection = new SqlConnection(@"Connection String");

            await connection.OpenAsync();

            string commandText;
            commandText = string.Format($"UPDATE ExpenseReports SET Status = '{exp.Status.ToString()}' WHERE Id = '{exp.Id}'");
            using SqlCommand command = new(commandText, connection);

            try { await command.ExecuteNonQueryAsync(); } catch (Exception ex) { _logger.LogError(ex, ex.Message); }
            await connection.CloseAsync();

            _logger.LogInformation($"Expense report status has been changed to: {exp.Status.ToString()}");
        }
    }
}