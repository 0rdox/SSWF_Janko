using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using Domain.Models.Enums;

namespace Domain.Models {
	public class DemoProducts {
		public int Id { get; set; }
		public List<Product> Products { get; set; }
		public TypeEnum Type { get; set; }

		public DemoProducts() {

		}

		public DemoProducts(List<Product> products, TypeEnum type) {
			Products = products;
			Type = type;
		}







	}
}
