using System.ComponentModel.DataAnnotations;
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


		//TODO CITY 
		public Student(string name, int studentNumber, DateTime dateOfBirth, string email, CityEnum city, string phone) {
			this.Name = name;
			this.StudentNumber = studentNumber;
			this.DateOfBirth = dateOfBirth;
			this.Email = email;
			this.City = city;
			this.Phone = phone;

			//Reservations = new List<Packet>();


			if (!CheckAge(dateOfBirth)) {
				throw new ArgumentException("Age Issue");
			}

		}

		public Student() {

		}


		private bool CheckAge(DateTime dob) {
			DateTime currentDate = DateTime.Now.Date; // Get the current date without time

			// TODO: Nederlands
			if (dob > currentDate) {
				throw new ArgumentException("Date of birth cannot be in the future.");
			}

			DateTime minimumAge = currentDate.AddYears(-16);
			if (dob > minimumAge) {
				throw new ArgumentException("User needs to be at least 16 years old.");
			}

			return true;
		}

	}
}