using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Models;


namespace Domain.Services {
	public interface IPacketRepository {

		IEnumerable<Packet> GetPackets();

		Task CreatePacket(Packet packet);

		Packet? GetPacketById(int id);

		Task ReservePacket(int packetId, int studentId);
	}
}
