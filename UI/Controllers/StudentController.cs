using Domain.Models;
using Domain.Models.Enums;
using Microsoft.AspNetCore.Mvc;
using Infrastructure.Repositories;
using Domain.Services;
using Microsoft.AspNetCore.Authorization;
using System.Net.Sockets;
using Domain;
using System.Net.Http.Headers;

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

        //token generation
		public async Task<IActionResult> Index() {
           // var identity = HttpContext.User.Identity;
            var student = _studentRepository.GetStudents().SingleOrDefault(a => a.Email == HttpContext.Session.GetString("UserEmail"));

            using var httpClient = new HttpClient();

            //TODO: change to azure
            var signInResponse = await httpClient.PostAsJsonAsync<SignInRequest>("https://ecotasteapi.azurewebsites.net/api/signin", new SignInRequest {
                Email = student.Email,
                Password = "Secret123",
            });
             
            // Read the response string as string

            var responseRaw = await signInResponse.Content.ReadAsStringAsync();
            Console.WriteLine(responseRaw);
            // And deserialize the string into the typed object.
            var typedResponse = System.Text.Json.JsonSerializer.Deserialize<SignInResponse>(responseRaw);

            if (signInResponse.IsSuccessStatusCode) {
                // Set the bearer token on the request. 
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", typedResponse.Token);

                return RedirectToAction("Packets", "Student");
            }
            return RedirectToAction("Index", "Home");
		}



		//ALL PACKETS
		[Authorize(Policy = "Student")]
        public IActionResult Packets() {
            var packets = _packetRepository.GetNotReservedPackets();
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

		//ALL RESERVATIONS
		[Authorize(Policy = "Student")]
		public IActionResult Reservations() {
			return View(_packetRepository.GetReservedPackets(GetStudentID()));
		}


		//MAKE RESERVATION
		[Authorize(Policy = "Student")]
        public async Task<IActionResult> Reserve(int packetId) {
            bool reservationResult = await _packetRepository.ReservePacketBool(packetId, GetStudentID());


            if (reservationResult) {
                return RedirectToAction("Reservations");
            }
            else {
                TempData["ReservationMessage"] = "U kunt dit pakket niet reserveren, u kunt maximaal 1 pakket reserveren per datum. Als u denkt dat dit niet klopt, herlaad de pagina";
                return RedirectToAction("Packets");
            }

        }






        //OTHER METHODS
        public int GetStudentID() {
         //   var loggedIn = HttpContext.User.Identity;


            var userName = HttpContext.Session.GetString("UserName");
            var email = HttpContext.Session.GetString("UserEmail");
            var id = HttpContext.Session.GetString("Id");

            return _studentRepository.GetStudents().FirstOrDefault(a => a.Email == email).Id;
        }

  

        public void IsStudentOverEighteen(int packetId) {



            var packet = _packetRepository.GetPacketById(packetId);
            var student = _studentRepository.GetStudentById(GetStudentID());
            int studentAge = student.CalculateAge(student.DateOfBirth);

            if (packet.OverEighteen && studentAge < 18) {
                ViewData["OverEighteen"] = false;
            }
            else {
                ViewData["OverEighteen"] = true;
            }
        }

        
    }
}