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



		// public IEnumerable<Packet> GetPackets() => _context.Packets.Where(a => a.ReservedById == null).OrderBy(c => c.DateTime);
		public IEnumerable<Packet> GetPackets() {
			var packets = _context.Packets.OrderBy(c => c.PickupTime);
			return packets;
		}

		public IEnumerable<Packet> GetNotReservedPackets() {
			var packets = _context.Packets.Where(a => a.ReservedById == null).OrderBy(c => c.PickupTime);
			return packets;
		}

		public IEnumerable<Packet> GetMyCanteenPackets(Canteen canteen) {
			var packetsOurs = _context.Packets
				.Where(b => b.CanteenNavigation == canteen)
				  .OrderBy(c => c.PickupTime);

			return packetsOurs;
		}

		public IEnumerable<Packet> GetOtherCanteenPackets(Canteen canteen) {
			var packetsOthers = _context.Packets
				.Where(b => b.CanteenNavigation != canteen)
				.OrderBy(c => c.PickupTime);

			return packetsOthers;
		}

		public Packet? GetPacketById(int id) => _context.Packets.Include(p => p.Products).FirstOrDefault(a => a.Id == id);
		public IEnumerable<Packet> GetReservedPackets(int studentId) => _context.Packets.Where(a => a.ReservedById == studentId).OrderBy(c => c.PickupTime);


		public async Task<bool> ReservePacketBool(int packetId, int studentId) {
			var packet = await _context.Packets.FirstOrDefaultAsync(a => a.Id == packetId);
			var student = await _context.Students.FirstOrDefaultAsync(a => a.Id == studentId);

			if (packet != null && student != null) {
				// Check if packet for that day is already reserved
				var existingReservation = await _context.Packets
					.FirstOrDefaultAsync(p =>
						p.ReservedById == studentId &&
						p.PickupTime.Date == packet.PickupTime.Date);

				if (existingReservation != null || student.CalculateAge(student.DateOfBirth) < 18) {
					return false;
				}
				else {
					packet.ReservedBy = student;
					await _context.SaveChangesAsync();
					return true;
				}
			}
			return false;
		}

		public async Task DeletePacket(Packet packet) {
			if (packet.ReservedById == null) {
				_context.Packets.Remove(packet);
				await _context.SaveChangesAsync();
			}
		}


		public async Task CreatePacket(string name, string price, DateTime pickupTime, string products, TypeEnum type, string imageUrl, Canteen canteen) {
			//Change . to ,
			if (price.Contains('.')) {
				price = price.Replace('.', ',');
			}


			//Get products
			List<Product> productObjects;
			if (!products.IsNullOrEmpty()) {
				productObjects = JsonConvert.DeserializeObject<List<Product>>(products);
			}
			else {
				productObjects = new List<Product>();
			}
			//Create packet
			Packet packet = new Packet(name, pickupTime, productObjects, canteen, Decimal.Parse(price), type, imageUrl);

			try {
				_context.Packets.Add(packet);
				await _context.SaveChangesAsync();
			}
			catch (Exception ex) {
				

			}

		}


		public async Task UpdatePacket(int packetId, string name, string price, DateTime pickupTime, string products, TypeEnum type, string imageUrl) {


			//Get packet before edit
			var existingPacket = _context.Packets.Include(p => p.Products).FirstOrDefault(p => p.Id == packetId);

			//Change . to ,
			if (price.Contains('.')) {
				price = price.Replace('.', ',');
			}



			existingPacket.Name = name;
			existingPacket.Price = Decimal.Parse(price);
			existingPacket.PickupTime = pickupTime;
			//Products
			if (products != null) {
				List<Product> productObjects = JsonConvert.DeserializeObject<List<Product>>(products);
				existingPacket.Products = productObjects!;
			}
			//--
			existingPacket.Type = type;
			existingPacket.ImageUrl = imageUrl;

			await _context.SaveChangesAsync();
		}




	}







}
