using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERS.Model
{
    public class ExpenseReport
    {
        public string Creator { get; set; } = "";
        public string Type { get; set; } = "";
        public double Amount { get; set; }
        public string Description { get; set; } = "";
        public TicketStatus Status { get; set; } = TicketStatus.Pending;

        public ExpenseReport() {}

        public ExpenseReport(string creator, string type, double amt, string desc)
        {
            Creator = creator;
            Type = type;
            Amount = amt;
            Description = desc;
        }
    }
}