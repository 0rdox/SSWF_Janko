using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Models.Enums;

namespace Domain.Models {
	public class Canteen {
		public int ID { get; set; }
		public CityEnum City { get; set; }
		public CanteenEnum Location { get; set; }
		
		public bool OffersHotMeals { get; set; }


		// Parameterless constructor for EF Core
		public Canteen() {
		
		}

		public Canteen(CityEnum city, CanteenEnum location, bool offersHotMeals) {
			City = city;
			Location = location;
			OffersHotMeals = offersHotMeals;

			int i = 0;
		}
	}
}
