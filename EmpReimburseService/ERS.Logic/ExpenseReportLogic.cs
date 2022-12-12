using ERS.Model;
using ERS.Repo;

namespace ERS.Logic
{
    public interface IExpenseReportLogic
    {
        Task<ExpenseReport> CreateTicket(Employee emp);
        Task<Queue<ExpenseReport>> GetReports(Employee emp, string filter);
        Task<ExpenseReport> ChangeReportStatus(Employee emp, string newStatus);
    }

    public class ExpenseReportLogic : IExpenseReportLogic
    {
        private readonly IExpenseReportRepo _repo;
        // private readonly ILogger<ExpenseReportLogic> _logger;

        public ExpenseReportLogic(IExpenseReportRepo repo)
        {
            _repo = repo;
            // _logger = logger;
        }

        public async Task<ExpenseReport> CreateTicket(Employee emp)
        {
            ExpenseReport newReport = await _repo.CreateReport(emp);

            return newReport;
        }

        public async Task<ExpenseReport> ChangeReportStatus(Employee emp, string newStatus)
        {
            ExpenseReport exp = new ExpenseReport();

            if (emp.IsManager)
            {
                Queue<ExpenseReport> reports = await _repo.GetReports(emp, newStatus);
                exp = reports.Peek();

                switch (newStatus)
                {
                    case "Approved":
                        exp.Status = TicketStatus.Approved;
                        break;
                    case "Rejected":
                        exp.Status = TicketStatus.Rejected;
                        break;
                }
            }
            else
            {
                throw new NotAManagerException("You are not approved to change status");
            }

            return exp;
        }

        public async Task<Queue<ExpenseReport>> GetReports(Employee emp, string filter)
        {
            Queue<ExpenseReport> reports = await _repo.GetReports(emp, filter);

            return reports;
        }
    }
}