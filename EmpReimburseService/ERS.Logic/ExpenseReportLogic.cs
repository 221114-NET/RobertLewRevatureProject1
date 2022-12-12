using Microsoft.Extensions.Logging;
using ERS.Model;
using ERS.Repo;

namespace ERS.Logic
{
    public interface IExpenseReportLogic
    {
        Task<ExpenseReport> CreateTicket(string email, string pw, ExpenseReport exp);
        Task<Queue<ExpenseReport>> GetReports(string email, string pw, string filter);
        Task<ExpenseReport> ChangeReportStatus(string email, string pw, string newStatus);
    }

    public class ExpenseReportLogic : IExpenseReportLogic
    {
        private readonly IExpenseReportRepo _expRepo;
        private readonly IEmployeeRepo _empRepo;
        private readonly ILogger<ExpenseReportLogic> _logger;

        public ExpenseReportLogic(IExpenseReportRepo expRepo, IEmployeeRepo empRepo, ILogger<ExpenseReportLogic> logger)
        {
            _expRepo = expRepo;
            _empRepo = empRepo;
            _logger = logger;
        }

        public async Task<ExpenseReport> CreateTicket(string email, string pw, ExpenseReport exp)
        {
            ExpenseReport newReport = await _expRepo.CreateReport(email, pw, exp);

            return newReport;
        }

        public async Task<ExpenseReport> ChangeReportStatus(string email, string pw, string newStatus)
        {
            ExpenseReport exp = new ExpenseReport();
            Queue<ExpenseReport> reports = await _expRepo.GetReports(email, pw, newStatus);
            Employee emp = await _empRepo.GetEmployee(email, pw);

            if (emp.IsManager)
            {
                exp = reports.Peek();

                switch (newStatus)
                {
                    case "Approved":
                        exp.Status = TicketStatus.Approved;
                        _expRepo.UpdateReport(exp);
                        break;
                    case "Rejected":
                        exp.Status = TicketStatus.Rejected;
                        _expRepo.UpdateReport(exp);
                        break;
                }
            }
            else
            {
                throw new NotAManagerException("You are not approved to change status");
            }

            return exp;
        }

        public async Task<Queue<ExpenseReport>> GetReports(string email, string pw, string filter)
        {
            Queue<ExpenseReport> reports = await _expRepo.GetReports(email, pw, filter);

            return reports;
        }
    }
}