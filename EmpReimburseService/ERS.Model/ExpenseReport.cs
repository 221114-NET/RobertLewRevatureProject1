using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERS.Model
{
    public class ExpenseReport
    {
        public Guid Id { get; set; }
        public Guid Creator { get; set; }
        public string Type { get; set; } = "";
        public double Amount { get; set; }
        public string Description { get; set; } = "";
        public TicketStatus Status { get; set; } = TicketStatus.Pending;

        public ExpenseReport() {}

        public ExpenseReport(Guid creator, string type, double amt, string desc)
        {
            Creator = creator;
            Type = type;
            Amount = amt;
            Description = desc;
        }

        public ExpenseReport(Guid id, Guid creator, string type, double amount, string description, TicketStatus status)
        {
            Id = id;
            Creator = creator;
            Type = type;
            Amount = amount;
            Description = description;
            Status = status;
        }
    }
}