using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Models;
using Domain.Models.Enums;

namespace Domain.Services {
    public interface IPacketRepository {

        IEnumerable<Packet> GetPackets();

        Task CreatePacket(Packet packet);

        Task DeletePacket(Packet packet);
        Task UpdatePacket(int packetId, Packet packet);

        Packet? GetPacketById(int id);

        Task ReservePacket(int packetId, int studentId);
    }
}
