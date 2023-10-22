using Domain.Models;
using Domain.Models.Enums;
using Microsoft.AspNetCore.Mvc;
using Infrastructure.Repositories;
using Domain.Services;
using Microsoft.AspNetCore.Authorization;
using System.Net.Sockets;

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

            //move to repo
            var packets = _packetRepository.GetPackets();
            //.Where(a => a.ReservedById == null)
            // .OrderBy(c => c.DateTime);

            return View(packets);
        }



        //PACKET DETAILS
        [Authorize(Policy = "Student")]
        public IActionResult PacketDetails(int id) {
            var packet = _packetRepository.GetPacketById(id);
            var demoList = _demoProductRepository.GetDemoProducts(packet.Type);
            IsStudentOverEighteen(id);


            return View((packet, demoList));
        }



        //MAKE RESERVATION
        [Authorize(Policy = "Student")]
        public async Task<IActionResult> Reserve(int packetId) {
            bool reservationResult = await _packetRepository.ReservePacketBool(packetId, GetStudentID());


            if (reservationResult) {
                // Redirect to "Reservations" if reservation was successful
                return RedirectToAction("Reservations");
            }
            else {
                // Set a message in TempData for the view
                TempData["ReservationMessage"] = "U heeft al een pakket op die dag gereserveerd";

                // Redirect back to the original page or another suitable page
                return RedirectToAction("Packets");
            }

        }

        //ALL RESERVATIONS
        [Authorize(Policy = "Student")]
        public IActionResult Reservations() {
            return View(_packetRepository.GetReservedPackets(GetStudentID()));
        }





        //OTHER
        public int GetStudentID() {
            var userName = HttpContext.Session.GetString("UserName");
            var email = HttpContext.Session.GetString("UserEmail");
            var id = HttpContext.Session.GetString("Id");

            return _studentRepository.GetStudents().FirstOrDefault(a => a.Email == email).Id;
        }


        public int CalculateAge(DateTime birthDate, DateTime currentDate) {
            int age = currentDate.Year - birthDate.Year;

            if (currentDate.Month < birthDate.Month || (currentDate.Month == birthDate.Month && currentDate.Day < birthDate.Day)) {
                age--;
            }

            return age;
        }


        public void IsStudentOverEighteen(int packetId) {
            var packet = _packetRepository.GetPacketById(packetId);
            var student = _studentRepository.GetStudentById(GetStudentID());
            int studentAge = CalculateAge(student.DateOfBirth, DateTime.Now);

            if (packet.OverEighteen && studentAge < 18) {
                ViewData["OverEighteen"] = false;
            }
            else {
                ViewData["OverEighteen"] = true;
            }
        }
    }
}