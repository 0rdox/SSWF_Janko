using Domain.Models;
using Domain.Models.Enums;
using Xunit;

namespace Domain.Tests {
    public class EmployeeTests {
        [Fact]
        public void Employee_Constructor_SetsProperties() {
            // Arrange
            int employeeID = 1;
            string name = "John Doe";
            string email = "john@example.com";
            Canteen canteen = new Canteen();

            // Act
            Employee employee = new Employee(employeeID, name, email, canteen);

            // Assert
            Assert.Equal(employeeID, employee.EmployeeID);
            Assert.Equal(name, employee.Name);
            Assert.Equal(email, employee.Email);
            Assert.Equal(canteen, employee.Canteen);
        }
    }
}
