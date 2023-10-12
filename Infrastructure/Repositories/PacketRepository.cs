using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Models;
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
                }
                else {
                    packet.ReservedBy = student;
                    await _context.SaveChangesAsync();
                }
            }
        }

    }
}
