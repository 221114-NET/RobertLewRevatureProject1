using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using ERS.Model;
using ERS.Business;

namespace Tests.Business
{
    public class RegistrationTests
    {
        [Fact]
        public void Register_NewUserWithValidEmailAndPWShouldRegister()
        {
            // Arrange
            string expected = "robert350@revature.net";

            // Act
            string actual = BusLayer.Register(new Employee("robert350@revature.net", "Password1!"));

            // Assert
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("Invalid email", "", "")]
        [InlineData("Invalid email", "Hoebag", "Password1!")]
        [InlineData("Invalid email", "user@gmail.com", "Password1!")]
        [InlineData("Invalid email", "User@revature.org", "Password1!")]
        public void Register_UserWithInvalidEmailShouldNotRegister(string expected, string email, string pw)
        {
            // Arrange

            // Act
            string actual = BusLayer.Register(new Employee(email, pw));

            // Assert
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("Invalid password", "guest@revature.net", "")]
        [InlineData("Invalid password", "Hoebag@revature.com", "adio9gf")]
        [InlineData("Invalid password", "user@revature.net", "adio9gfAte")]
        
        public void Register_NewUserWithInvalidPWShouldNotRegister(string expected, string email, string pw)
        {
            // Arrange

            // Act
            string actual = BusLayer.Register(new Employee(email, pw));

            // Assert
            Assert.Equal(expected, actual);
        }

        /* [Fact]
        public void Register_ExistingUserShouldNotRegister()
        {
            // Arrange
            string expected = "User already exists";

            // Act
            string actual = BusLayerClass.Register(new Employee("jisaburo.lew@gmail.com", "MudButt69!"));

            // Assert
            Assert.Equal(expected, actual);
        } */
    }
}