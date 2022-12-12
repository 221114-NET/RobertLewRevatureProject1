using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ERS.Logic;
using ERS.Model;
using ERS.Repo;
using Xunit;

namespace Tests.Api
{
    public class TicketTests
    {
        [Fact]
        public async void ExpenseReport_CanBeCreated()
        {
            // Arrange
            IExpenseReportRepo i = new MockExpReportRepo();
            ExpenseReportLogic bl = new ExpenseReportLogic(i);
            Guid expected = new Guid("c40dab3d-3279-4b57-97f9-6f594137d99d");

            // Act
            ExpenseReport actual = await bl.CreateTicket(new Employee());

            // Assert
            Assert.Equal(expected, actual.Creator);
        }

        /* [Fact]
        public async void ExpenseReport_EmployeeNotLogged()
        {
            // Arrange
            IExpenseReportRepo i = new MockExpReportRepo();
            ExpenseReportLogic bl = new ExpenseReportLogic(i);

            // Act
            string expected = "Employee is not logged in";
            var ex = await Assert.ThrowsAsync<EmployeeNotLoggedInException>(() => bl.CreateTicket(new Employee(), new ExpenseReport()));

            // Assert
            Assert.Equal(expected, ex.Message);
        } */

        [Fact]
        public async void ExpenseReports_ManagerCanSeeQueue()
        {
            // Arrange
            IExpenseReportRepo i = new MockExpReportRepo();
            ExpenseReportLogic bl = new ExpenseReportLogic(i);
            Queue<ExpenseReport> reports = await bl.GetReports(new Employee(true), "");


            // Act
            string expected = "Unique";
            string actual = reports.Peek().Type;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async void ExpenseReports_NonManagerCanSeeOwnReports()
        {
            // Arrange
            IExpenseReportRepo i = new MockExpReportRepo();
            ExpenseReportLogic bl = new ExpenseReportLogic(i);
            Queue<ExpenseReport> reports = await bl.GetReports(new Employee(new Guid("c40dab3d-3279-4b57-97f9-6f594137d99d"), "", "", "", ""), "");


            // Act
            string expected = "Travel";
            string actual = reports.Peek().Type;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async void ExpenseReport_ManagerCanChangeStatusToApproved()
        {
            // Arrange
            IExpenseReportRepo i = new MockExpReportRepo();
            ExpenseReportLogic bl = new ExpenseReportLogic(i);
            ExpenseReport exp = await bl.ChangeReportStatus(new Employee(true), "Approved");

            // Act
            TicketStatus expected = TicketStatus.Approved;
            TicketStatus actual = exp.Status;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async void ExpenseReport_ManagerCanChangeStatusToRejected()
        {
            // Arrange
            IExpenseReportRepo i = new MockExpReportRepo();
            ExpenseReportLogic bl = new ExpenseReportLogic(i);
            ExpenseReport exp = await bl.ChangeReportStatus(new Employee(true), "Rejected");

            // Act
            TicketStatus expected = TicketStatus.Rejected;
            TicketStatus actual = exp.Status;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async void ExpenseReport_NonManagerCannotChangeStatus()
        {
            // Arrange
            IExpenseReportRepo i = new MockExpReportRepo();
            ExpenseReportLogic bl = new ExpenseReportLogic(i);

            // Act
            string expected = "You are not approved to change status";
            var ex = await Assert.ThrowsAsync<NotAManagerException>(() => bl.ChangeReportStatus(new Employee(), "Approved"));

            // Assert
            Assert.Equal(expected, ex.Message);
        }
    }
}