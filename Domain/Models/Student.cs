using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Domain.Models.Enums;

namespace Domain.Models {
	public class Student {

		public int Id { get; set; }

		[Required]
		public string Name { get; set; }


		public int StudentNumber { get; set; }

		[Required]
		public DateTime DateOfBirth { get; set; }

		[Required]
		public string Email { get; set; }


		[EnumDataType(typeof(CityEnum))]
		public CityEnum City { get; set; }

		public string Phone { get; set; } = null!;
	
		public List<Packet> Reservations { get; set; }



		public Student(string name, int studentNumber, DateTime dateOfBirth, string email, CityEnum city, string phone) {
			this.Name = name;
			this.StudentNumber = studentNumber;
			this.DateOfBirth = dateOfBirth;
			this.Email = email;
			this.City = city;
			this.Phone = phone;

			int age = CalculateAge(dateOfBirth);
			if (age < 16) {
				throw new ArgumentException("Gebruiker moet minimaal 16 jaar oud zijn");
			} else if (age < 0) {
				throw new ArgumentException("Geboortedatum kan niet in de toekomst liggen.");
			} 
		}

		public Student() {

		}


		public int CalculateAge(DateTime birthDate) {
			DateTime currentDate = DateTime.Now;

			int age = currentDate.Year - birthDate.Year;

			if (currentDate.Month < birthDate.Month || (currentDate.Month == birthDate.Month && currentDate.Day < birthDate.Day)) {
				age--;
			}

			return age;
		}
	}
}