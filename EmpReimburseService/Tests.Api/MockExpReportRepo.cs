using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ERS.Model;
using ERS.Repo;

namespace Tests.Api
{
    public class MockExpReportRepo : IExpenseReportRepo
    {
        public async Task<ExpenseReport> CreateReport(string email, string pw, ExpenseReport exp)
        {
            Guid Id = new Guid("c40dab3d-3279-4b57-97f9-6f594137d99d");

            ExpenseReport tempExpReport = new ExpenseReport();
            tempExpReport.Amount = 100.00;
            tempExpReport.Creator = Id;
            tempExpReport.Description = "Description";
            tempExpReport.Type = "Travel";

            return tempExpReport;
        }

        public async Task<Queue<ExpenseReport>> GetReports(string email, string pw, string filter)
        {
            ExpenseReport rep1 = new ExpenseReport(new Guid(), "Unique", 250.0, "Unique description");
            ExpenseReport rep2 = new ExpenseReport(new Guid("c40dab3d-3279-4b57-97f9-6f594137d99d"), "Travel", 500.0, "Hotel and Airfare");
            ExpenseReport rep3 = new ExpenseReport(new Guid(), "Travel", 750.0, "Hotel and Airfare");
            ExpenseReport rep4 = new ExpenseReport(new Guid("c40dab3d-3279-4b57-97f9-6f594137d99d"), "Food", 100.0, "Client dinner meeting");
            ExpenseReport rep5 = new ExpenseReport(new Guid("c40dab3d-3279-4b57-97f9-6f594137d99d"), "Gas", 60.0, "Gas for driving to client meeting");

            Queue<ExpenseReport> reports = new Queue<ExpenseReport>();
            reports.Enqueue(rep1);
            reports.Enqueue(rep2);
            reports.Enqueue(rep3);
            reports.Enqueue(rep4);
            reports.Enqueue(rep5);

            Employee emp = new Employee(email, pw);
            emp.Id = new Guid("c40dab3d-3279-4b57-97f9-6f594137d99d");

            if (!emp.IsManager)
            {
                List<ExpenseReport> repList = reports.ToList<ExpenseReport>();
                reports.Clear();
                foreach (var report in repList)
                {
                    if (report.Creator == emp.Id)
                    {
                        reports.Enqueue(report);
                    }
                }
            }
            
            return reports;
        }

        public void UpdateReport(ExpenseReport exp)
        {
            throw new NotImplementedException();
        }
    }
}