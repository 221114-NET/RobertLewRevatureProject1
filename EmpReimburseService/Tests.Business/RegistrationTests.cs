using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using ERS.Model;
using ERS.Logic;
using ERS.Repo;

namespace Tests.Business
{
    public class RegistrationTests
    {
        [Fact]
        public async void Register_UserWithValidEmailAndPWShouldRegister()
        {
            // Arrange
            IRepository i = new MockRepository();
            BusinessLogic bl = new BusinessLogic(i);
            string expected = "robert350@revature.net";

            // Act
            Employee actual = await bl.Register(new Employee("robert350@revature.net", "Password1!"));

            // Assert
            Assert.Equal(expected, actual.Email);
        }

        [Theory]
        [InlineData("Invalid email", "", "")]
        [InlineData("Invalid email", "Hoebag", "Password1!")]
        [InlineData("Invalid email", "user@gmail.com", "Password1!")]
        [InlineData("Invalid email", "User@revature.org", "Password1!")]
        public async void Register_UserWithInvalidEmailShouldNotRegister(string expected, string email, string pw)
        {
            // Arrange
            IRepository i = new MockRepository();
            BusinessLogic bl = new BusinessLogic(i);

            // Act
            var ex = await Assert.ThrowsAsync<InvalidEmailException>(() => bl.Register(new Employee(email, pw)));

            // Assert
            Assert.Equal(expected, ex.Message);
        }

        [Theory]
        [InlineData("Invalid password", "guest@revature.com", "")]
        [InlineData("Invalid password", "Hoebag@revature.com", "adio9gf")]
        [InlineData("Invalid password", "user@revature.com", "adio9gfAte")]
        [InlineData("Invalid password", "guest@revature.net", "")]
        [InlineData("Invalid password", "Hoebag@revature.net", "adio9gf")]
        [InlineData("Invalid password", "user@revature.net", "adio9gfAte")]
        
        public async void Register_UserWithInvalidPWShouldNotRegister(string expected, string email, string pw)
        {
            // Arrange
            IRepository i = new MockRepository();
            BusinessLogic bl = new BusinessLogic(i);

            // Act
            var ex = await Assert.ThrowsAsync<InvalidEmailException>(() => bl.Register(new Employee(email, pw)));

            // Assert
            Assert.Equal(expected, ex.Message);
        }

        [Fact]
        public async void Register_ExistingUserShouldNotRegister()
        {
            // Arrange
            IRepository i = new MockRepository();
            BusinessLogic bl = new BusinessLogic(i);
            Employee e = new Employee("joe123@revature.net", "Password1!");

            // Act
            string expected = "Employee already exists";
            var ex = await Assert.ThrowsAsync<EmployeeAlreadyExistsException>(() => bl.Register(e));

            // Assert
            Assert.Equal(expected, ex.Message);
        }

        [Fact]
        public async void Login_EmployeeCanLogin()
        {
            // Arrange
            IRepository i = new MockRepository();
            BusinessLogic bl = new BusinessLogic(i);

            // Act
            string expected = "joe123@revature.net";
            Employee e = await bl.Login(new Employee("joe123@revature.net", "Password1!"));

            // Assert
            Assert.Equal(expected, e.Email);
        }

        [Fact]
        public async void Login_EmployeeDoesNotExist()
        {
            // Arrange
            IRepository i = new MockRepository();
            BusinessLogic bl = new BusinessLogic(i);

            // Act
            string expected = "No such employee exists";
            var ex = await Assert.ThrowsAsync<EmployeeNotFoundException>(() => bl.Login(new Employee("robert350@revature.net", "Password1!")));

            // Assert
            Assert.Equal(expected, ex.Message);
        }
    }
}