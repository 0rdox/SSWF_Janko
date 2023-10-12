using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models {
	public class Product {
		public int Id { get; set; }
		public string Name { get; set; }
		public bool Alcohol { get; set; }
		public string ImageUrl { get; set; }

		public Packet? Packet { get; set; }

		public int? PacketId { get; set; }

		public DemoProducts? DemoProducts { get; set; }
		public int? DemoProductsId { get; set; }

		public Product(string name, bool alcohol, string imageUrl) {
			Name = name;
			Alcohol = alcohol;
			ImageUrl = imageUrl;
		}

		public Product() {

		}








	}
}
