using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Models;


namespace Domain.Services {
	public interface ICanteenRepository {
		IEnumerable<Canteen> GetCanteens();

		Canteen GetCanteenById(int id);

		


	}
}
