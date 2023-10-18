using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Models;
using Domain.Models.Enums;
using Domain.Services;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories {
    public class PacketRepository : IPacketRepository {
        private readonly AppDBContext _context;

        public PacketRepository(AppDBContext context) {
            _context = context;
        }

        public async Task CreatePacket(Packet packet) {
            _context.Packets.Add(packet);
            await _context.SaveChangesAsync();
        }

        public IEnumerable<Packet> GetPackets() {
            return _context.Packets;
        }

        public Packet? GetPacketById(int id) {
            var products = _context.Products.Where(p => p.PacketId == id);

            return _context.Packets.FirstOrDefault(a => a.Id == id);
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
                } else {
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

    }
}
