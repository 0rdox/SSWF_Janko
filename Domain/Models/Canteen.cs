using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Models.Enums;

namespace Domain.Models {
	public class Canteen {
		[Key]
		public int Id { get; set; }
		public CityEnum City { get; set; }
		public CanteenEnum Location { get; set; }
		
		public bool OffersHotMeals { get; set; }

		public Canteen() {
		
		}

		public Canteen(CityEnum city, CanteenEnum location, bool offersHotMeals) {
			City = city;
			Location = location;
			OffersHotMeals = offersHotMeals;

		}
	}
}
