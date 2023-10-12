using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Models;
using Domain.Models.Enums;

namespace Domain.Services {
    public interface IDemoProductRepository {

        IEnumerable<Product> GetDemoProducts(TypeEnum type);



    }
}
