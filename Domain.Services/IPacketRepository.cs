using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Models;
using Domain.Models.Enums;

namespace Domain.Services {
    public interface IPacketRepository {

		Packet? GetPacketById(int id);
		IEnumerable<Packet> GetPackets();

        IEnumerable<Packet> GetNotReservedPackets();

		IEnumerable<Packet> GetReservedPackets(int studentId);
        IEnumerable<Packet> GetMyCanteenPackets(Canteen canteen);
        IEnumerable<Packet> GetOtherCanteenPackets(Canteen canteen);


        Task CreatePacket(string name, string price, DateTime pickupTime, string products, TypeEnum type, string imageUrl, Canteen canteen);
        Task DeletePacket(Packet packet);
        Task UpdatePacket(int packetId, string name, string price, DateTime pickupTime, string products, TypeEnum type, string imageUrl);

   

   
        Task<bool> ReservePacketBool(int packetId, int studentId);
    }
}
