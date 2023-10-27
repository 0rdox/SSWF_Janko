using Domain.Models;
using Domain.Models.Enums;
using System;
using System.ComponentModel.DataAnnotations;
using Xunit;

namespace Domain.Tests {
	public class StudentTests {
		[Fact]
		public void Student_Constructor_SetsProperties() {
			// Arrange
			string name = "John Doe";
			int studentNumber = 12345;
			DateTime dateOfBirth = new DateTime(2000, 1, 1);
			string email = "john.doe@example.com";
			CityEnum city = CityEnum.Breda;
			string phone = "123-456-7890";

			// Act
			Student student = new Student(name, studentNumber, dateOfBirth, email, city, phone);

			// Assert
			Assert.Equal(name, student.Name);
			Assert.Equal(studentNumber, student.StudentNumber);
			Assert.Equal(dateOfBirth, student.DateOfBirth);
			Assert.Equal(email, student.Email);
			Assert.Equal(city, student.City);
			Assert.Equal(phone, student.Phone);
		}

		[Fact]
		public void Student_InvalidAge_ThrowsArgumentException() {
			// Arrange
			string name = "John Doe";
			int studentNumber = 12345;
			DateTime dateOfBirth = DateTime.Now.AddDays(1); // Future date of birth
			string email = "john.doe@example.com";
			CityEnum city = CityEnum.Breda;
			string phone = "123-456-7890";

			// Act & Assert
			Assert.Throws<ArgumentException>(() => new Student(name, studentNumber, dateOfBirth, email, city, phone));
		}

		[Fact]
		public void Student_InvalidAge_MinimumAge16_ThrowsArgumentException() {
			// Arrange
			string name = "John Doe";
			int studentNumber = 12345;
			DateTime dateOfBirth = DateTime.Now.AddYears(-15); // Below 16 years old
			string email = "john.doe@example.com";
			CityEnum city = CityEnum.Breda;
			string phone = "123-456-7890";

			// Act & Assert
			Assert.Throws<ArgumentException>(() => new Student(name, studentNumber, dateOfBirth, email, city, phone));
		}

		[Fact]
		public void Student_ValidAge_DoesNotThrowException() {
			// Arrange
			string name = "John Doe";
			int studentNumber = 12345;
			DateTime dateOfBirth = DateTime.Now.AddYears(-20); // 20 years old
			string email = "john.doe@example.com";
			CityEnum city = CityEnum.Breda;
			string phone = "123-456-7890";

			// Act
			Exception ex = Record.Exception(() => new Student(name, studentNumber, dateOfBirth, email, city, phone));

			// Assert
			Assert.Null(ex);
		}

		[Theory]
		[InlineData("2000-01-01", 23)]
		[InlineData("1995-05-15", 28)]
		[InlineData("2010-08-10", 13)]
		public void CalculateAge_ShouldReturnCorrectAge(string birthDateStr, int expectedAge) {
			// Arrange
			DateTime birthDate = DateTime.Parse(birthDateStr);
			DateTime currentDate = new DateTime(2023, 09, 14); 
			var student = new Student(); 

			// Act
			int actualAge = student.CalculateAge(birthDate);

			// Assert
			Assert.Equal(expectedAge, actualAge);
		}
	}
}
