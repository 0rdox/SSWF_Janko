using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Models;


namespace Domain.Services {
    public interface IProductRepository {

        IEnumerable<Product> GetProducts();

        Task CreateProduct(Product product);
        //Product GetProductById(int id);


    }
}
