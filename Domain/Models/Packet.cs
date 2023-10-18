using Domain.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Reflection.Emit;
using System.Runtime.ConstrainedExecution;
using System.Security.Cryptography.Xml;

namespace Domain.Models {
    public class Packet {

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "Naam is verplicht")]
        public string Name { get; set; }
        public CityEnum City { get; set; }
        public CanteenEnum Canteen { get; set; }
        public Canteen CanteenNavigation { get; set; }

        [Required(ErrorMessage = "Pickup time is required.")]
        [Display(Name = "Ophaaltijd")]
        [DataType(DataType.DateTime)]

        public DateTime DateTime { get; set; } //ophalen
        public DateTime MaxDateTime { get; set; } //max tijd ophalen
        public bool OverEighteen { get; set; }

        //Many to many
        public List<Product> Products { get; set; }
        [Column(TypeName = "decimal(5, 2)")]

        [Required(ErrorMessage = "Prijs is verplicht")]

        public decimal Price { get; set; }
        public TypeEnum Type { get; set; } //enum

        public int? ReservedById { get; set; }
        public Student? ReservedBy { get; set; }


        public string ImageUrl { get; set; }

        //public List<Product> DemoProducts { get; set; }

        public Packet(string name, decimal price, string imageUrl, TypeEnum type) {
            this.Name = name;
            this.Price = price;
            this.ImageUrl = imageUrl;
            this.Type = type;
            ReservedBy = null;
            //DemoProducts = AddDemoProducts();
        }

        public Packet() {

        }
        public Packet(string name, DateTime dateTime, List<Product> products, decimal price, TypeEnum type, string imageUrl) {
            Name = name;


            //if (!PickupTimeIsValid(dateTime)) {
            //    throw new ArgumentException("Invalid pickup time. Pickup time must be within 48 hours from now.");
            //}


            DateTime = dateTime;
            //Max tijd van ophalen is 6u na originele ophal moment
            MaxDateTime = dateTime.AddHours(6);


            //Producten worden gecheckt op of het alcohol bevat, zo ja? packet is 18+
            OverEighteen = IsOverEighteen(products);



            Products = products;
            Price = price;
            Type = type;
            ImageUrl = imageUrl;
            ReservedBy = null;
        }


        //Do this after clicking submit?
        public bool IsOverEighteen(List<Product> products) {
            foreach (var product in products) {
                if (product.Alcohol) {
                    return true;
                }
            }
            return false;
        }


        public void Reserve(Student student) {
            ReservedBy = student;
            ReservedById = student.Id;
        }


        public bool PickupTimeIsValid(DateTime pickupTime) {
            var maxAllowedTime = DateTime.Now.AddHours(48);
            return pickupTime >= DateTime.Now && pickupTime <= maxAllowedTime;
        }


    }
}




