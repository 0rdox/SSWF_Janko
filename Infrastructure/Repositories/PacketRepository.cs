using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Models;
using Domain.Models.Enums;
using Domain.Services;
using Infrastructure.Data;
using Infrastructure.Migrations.AppDB;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace Infrastructure.Repositories {
    public class PacketRepository : IPacketRepository {
        private readonly AppDBContext _context;

        public PacketRepository(AppDBContext context) {
            _context = context;
        }



        public IEnumerable<Packet> GetPackets() => _context.Packets;


        public Packet? GetPacketById(int id) {
            return _context.Packets
     .Include(p => p.Products) // Eager loading of Products
     .FirstOrDefault(a => a.Id == id);
        }

        public async Task ReservePacket(int packetId, int studentId) {
            var packet = await _context.Packets.FirstOrDefaultAsync(a => a.Id == packetId);
            var student = await _context.Students.FirstOrDefaultAsync(a => a.Id == studentId);

            if (packet != null && student != null) {

                //Check if student already has reservation on pickup day
                var existingReservation = await _context.Packets
                    .FirstOrDefaultAsync(p =>
                        p.ReservedById == studentId &&
                        p.DateTime.Date == packet.DateTime.Date);

                if (existingReservation != null) {
                    //handle exception
                }
                else {
                    packet.ReservedBy = student;
                    await _context.SaveChangesAsync();
                }
            }
        }

        public async Task DeletePacket(Packet packet) {
            _context.Packets.Remove(packet);
            await _context.SaveChangesAsync();
        }

        public async Task UpdatePacket(int packetId, string? newName = null, decimal? newPrice = null, DateTime? newPickupTime = null, List<Product>? newProducts = null, TypeEnum? newType = null, string? newImageUrl = null) {
            //var packet = await _context.Packets
            //    .Include(p => p.Products)
            //    .FirstOrDefaultAsync(p => p.Id == packetId);

            var packet = GetPacketById(packetId);

            if (packet != null) {
                if (newName != null) {
                    packet.Name = newName;
                }

                if (newPrice != null) {
                    packet.Price = newPrice.Value;
                }

                if (newPickupTime != null) {
                    packet.DateTime = newPickupTime.Value;
                    packet.MaxDateTime = newPickupTime.Value.AddHours(6);
                }

                if (newProducts != null) {
                    packet.Products.Clear();
                    if (newProducts.Any()) {
                        packet.Products.AddRange(newProducts);
                    }
                }

                if (newType != null) {
                    packet.Type = newType.Value;
                }

                if (newImageUrl != null) {
                    packet.ImageUrl = newImageUrl;
                }

                _context.Entry(packet).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
        }



        public IEnumerable<Packet> GetMyCanteenPackets(Canteen canteen) {
            //make _packetRepository.GetMyPackets()
            var packetsOurs = _context.Packets
            .Where(a => a.ReservedById == null)
                .Where(b => b.CanteenNavigation == canteen)
                  .OrderBy(c => c.DateTime);

            return packetsOurs;


        }

        public IEnumerable<Packet> GetOtherCanteenPackets(Canteen canteen) {

            //make _packetRepository.GetOtherPackets()
            var packetsOthers = _context.Packets
            .Where(a => a.ReservedById == null)
                .Where(b => b.CanteenNavigation != canteen)
                .OrderBy(c => c.DateTime);



            return packetsOthers;
        }

        public async Task CreatePacket(string name, string price, DateTime pickupTime, string products, TypeEnum type, string imageUrl, Canteen canteen) {

            if (price.Contains('.')) {
                price = price.Replace('.', ',');
            }



            List<Product> productObjects;
            if (!products.IsNullOrEmpty()) {
                productObjects = JsonConvert.DeserializeObject<List<Product>>(products);
            }
            else {
                productObjects = new List<Product>();
            }
            Packet packet = new Packet(name, pickupTime, productObjects, canteen, Decimal.Parse(price), type, imageUrl);

            _context.Packets.Add(packet);
            await _context.SaveChangesAsync();

        }

 
        public async Task UpdatePacket(int packetId, string name, string price, DateTime pickupTime, string products, TypeEnum type, string imageUrl) {


            //Get packet before edit
            var existingPacket = _context.Packets.Include(p => p.Products).FirstOrDefault(p => p.Id == packetId);


            //fix price
            if (price.Contains('.')) {
                price = price.Replace('.', ',');
            }



            existingPacket.Name = name;
            existingPacket.Price = Decimal.Parse(price);
            existingPacket.DateTime = pickupTime;
            //Products
            if (products != null) {
                List<Product> productObjects = JsonConvert.DeserializeObject<List<Product>>(products);
                existingPacket.Products = productObjects;
            }
            //--
            existingPacket.Type = type;
            existingPacket.ImageUrl = imageUrl;

        

            await _context.SaveChangesAsync();
        }
    }
}
