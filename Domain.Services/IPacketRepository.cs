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

        IEnumerable<Packet> GetMyCanteenPackets(Canteen canteen);
        IEnumerable<Packet> GetOtherCanteenPackets(Canteen canteen);




        Task CreatePacket(string name, string price, DateTime pickupTime, string products, TypeEnum type, string imageUrl, Canteen canteen);

        Task DeletePacket(Packet packet);
        Task UpdatePacket(int packetId, string name, string price, DateTime pickupTime, string products, TypeEnum type, string imageUrl);


        Packet? GetPacketById(int id);

        Task ReservePacket(int packetId, int studentId);
    }
}
