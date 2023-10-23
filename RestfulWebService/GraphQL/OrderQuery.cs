using Domain.Models;
using Domain.Services;

namespace RestfulWebService.GraphQL {
    public class OrderQuery {

        private readonly IPacketRepository _packetRepository;
        private readonly IProductRepository _productRepository;

        public OrderQuery(IPacketRepository packetRepository, IProductRepository productRepository) {
            _packetRepository = packetRepository;
            _productRepository = productRepository;
        }
        //lijst met pakketten en producten

        
        public IEnumerable<Packet> GetAllPackets() {
            return _packetRepository.GetPackets();
        }    
        public Packet GetPacketById(int id) {
            return _packetRepository.GetPacketById(id);
        }

        public IEnumerable<Product> GetAllProducts() {
            return _productRepository.GetProducts();

        }

    }
}
