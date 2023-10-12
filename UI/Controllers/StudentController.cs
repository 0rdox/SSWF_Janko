using Domain.Models;
using Domain.Models.Enums;
using Microsoft.AspNetCore.Mvc;
using Infrastructure.Repositories;
using Domain.Services;
using Microsoft.AspNetCore.Authorization;

namespace UI.Controllers {
	public class StudentController : Controller {

		private readonly IPacketRepository _packetRepository;
		private readonly IStudentRepository _studentRepository;
		private readonly IDemoProductRepository _demoProductRepository;

		public StudentController(IPacketRepository packetRepository, IStudentRepository studentRepository, IDemoProductRepository demoProductRepository) {
			_packetRepository = packetRepository;
			_studentRepository = studentRepository;
			_demoProductRepository = demoProductRepository;
		}




		//ALL PACKETS
		[Authorize(Policy = "Student")]
		public IActionResult Packets() {
			var packets = _packetRepository.GetPackets()
				.Where(a => a.ReservedById == null);

			return View(packets);
		}



		//PACKET DETAILS
		[Authorize(Policy = "Student")]
		public IActionResult PacketDetails(int id) {
			var packet = _packetRepository.GetPacketById(id);
			var demoList = _demoProductRepository.GetDemoProducts(packet.Type);

			
			if (packet == null) {
				return View("Packets");
			}
			return View((packet, demoList));
		}


		//MAKE RESERVATION
		[Authorize(Policy = "Student")]
		public async Task<IActionResult> Reserve(int packetId) {
			//CHECK IF PACKET IS 18+ -> CHECK IF AGE IS 18+
			
			await _packetRepository.ReservePacket(packetId, GetStudentID());

			//TODO: get student id

			return RedirectToAction("Reservations");

		}

		//ALL RESERVATIONS
		[Authorize(Policy = "Student")]
		public IActionResult Reservations() {
			var packets = _packetRepository.GetPackets()
				.Where(a => a.ReservedById == GetStudentID());

			return View(packets);
		}





		//OTHER
		public int GetStudentID() {
			var userName = HttpContext.Session.GetString("UserName");
			var email = HttpContext.Session.GetString("UserEmail");
			var id = HttpContext.Session.GetString("Id");

			return _studentRepository.GetStudents().FirstOrDefault(a => a.Email == email).Id;
		}
	}
}