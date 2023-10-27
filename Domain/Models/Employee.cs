using System.ComponentModel.DataAnnotations;
using Domain.Models.Enums;

namespace Domain.Models {
	public class Employee 
		{

		[Key]
		public int EmployeeID { get; set; }
		public string Name { get; set; }

		public string Email { get; set; }
		public Canteen Canteen { get; set; }
		public int CanteenId { get; set; }


		public Employee() {
		}

		public Employee(int employeeID, string name, string email, Canteen canteen) {
			EmployeeID = employeeID;
			Name = name;
			Email = email;
			Canteen = canteen;
		}

	}
}
