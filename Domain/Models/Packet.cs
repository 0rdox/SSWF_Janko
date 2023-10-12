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
        public string Name { get; set; }
        public CityEnum City { get; set; }
        public CanteenEnum Canteen { get; set; }
        public Canteen CanteenNavigation { get; set; }
        public DateTime DateTime { get; set; } //ophalen
        public DateTime MaxDateTime { get; set; } //max tijd ophalen
        public bool OverEighteen { get; set; }

        //Many to many
        public List<Product> Products { get; set; }
        [Column(TypeName = "decimal(5, 2)")]
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


        public List<Product> AddDemoProducts() {
            List<Product> products = new List<Product>();

            switch (this.Type) {
                case TypeEnum.Drankpakket:
                //products.Add(new Product { Id = 10, Name = "Bier", Alcohol = true, ImageUrl = "https://thumbs.dreamstime.com/b/koud-bier-43280582.jpg" });
                //products.Add(new Product { Id = 11,  Name = "Wijn", Alcohol = true, ImageUrl = "https://static.vecteezy.com/ti/gratis-vector/p3/7324461-wijnglazen-met-witte-wijn-illustratie-van-wijnglazen-geisoleerd-op-witte-achtergrond-gratis-vector.jpg" });
                //products.Add(new Product { Id = 12, Name = "Whisky", Alcohol = true, ImageUrl = "https://img.freepik.com/premium-photo/glas-schotse-whisky-en-ijs-op-een-witte-achtergrond_38145-1376.jpg?w=2000" });


                break;
                case TypeEnum.Broodpakket:

                Product prod1 = new Product("Worstenbroodje", false, "https://www.hetzeilbergsbakkertje.nl/wp-content/uploads/JPM0008-Worstenbroodje-open.jpg");
                Product prod2 = new Product("Worstenbroodje", false, "https://www.hetzeilbergsbakkertje.nl/wp-content/uploads/JPM0008-Worstenbroodje-open.jpg");
                Product prod3 = new Product("Worstenbroodje", false, "https://www.hetzeilbergsbakkertje.nl/wp-content/uploads/JPM0008-Worstenbroodje-open.jpg");
                products.Add(prod1);
                products.Add(prod2);
                products.Add(prod3);

                //products.Add(new Product { Id = 1, Name = "Worstenbroodje", Alcohol = false, ImageUrl = "https://www.hetzeilbergsbakkertje.nl/wp-content/uploads/JPM0008-Worstenbroodje-open.jpg" });
                //products.Add(new Product { Id = 2, Name = "Saucijzenbroodje", Alcohol = false, ImageUrl = "https://www.bakkerijvoortman.nl/wp/wp-content/uploads/2015/02/saucbr..png" });
                //products.Add(new Product { Id = 3, Name = "Kaassoufflé", Alcohol = false, ImageUrl = "https://www.ahealthylife.nl/wp-content/uploads/2021/06/Kaassouffle_voedingswaarde-1.jpg" });
                break;

                case TypeEnum.WarmeMaaltijd:
                break;
                case TypeEnum.Unknown:
                break;
            }

            return products;
        }
    }
}




