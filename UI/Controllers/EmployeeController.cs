using Domain.Models;
using Domain.Models.Enums;
using Microsoft.AspNetCore.Mvc;
using Infrastructure.Repositories;
using Domain.Services;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.Net.Http.Json;
using Newtonsoft.Json;
using Infrastructure.Migrations.AppDB;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using Domain;
using System.Net.Http.Headers;

namespace UI.Controllers {
	public class EmployeeController : Controller {

		private readonly IPacketRepository _packetRepository;
		private readonly IEmployeeRepository _employeeRepository;
		private readonly ICanteenRepository _canteenRepository;
		private readonly IDemoProductRepository _demoProductRepository;


		public EmployeeController(IPacketRepository packetRepository, IEmployeeRepository employeeRepository, ICanteenRepository canteenRepository, IDemoProductRepository demoProductRepository) {
			_packetRepository = packetRepository;
			_employeeRepository = employeeRepository;
			_canteenRepository = canteenRepository;
			_demoProductRepository = demoProductRepository;
		}


		//token generation
		public async Task<IActionResult> Index() {
			//var identity = HttpContext.User.Identity;
			var employee = _employeeRepository.GetEmployees().SingleOrDefault(a => a.Email == HttpContext.Session.GetString("UserEmail"));

			using var httpClient = new HttpClient();

			var signInResponse = await httpClient.PostAsJsonAsync<SignInRequest>("ecotaste.azurewebsites.net/api/signin", new SignInRequest {
				Email = employee.Email,
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

				return RedirectToAction("Packets", "Employee");
			}

			return RedirectToAction("Index", "Home");

		}


		//ALL PACKETS
		[Authorize(Policy = "Employee")]
		public IActionResult Packets() {
			var packetsOurs2 = _packetRepository.GetMyCanteenPackets(GetCanteen());
			var packetsOthers2 = _packetRepository.GetOtherCanteenPackets(GetCanteen());

			return View((packetsOurs2, packetsOthers2));
		}


		//PACKET DETAILS
		[Authorize(Policy = "Employee")]
		public IActionResult PacketDetails(int id) {
			var packet = _packetRepository.GetPacketById(id);
			var demoList = _demoProductRepository.GetDemoProducts(packet.Type);
			Canteen canteen = GetCanteen();
			bool canEdit = packet.CanteenNavigation == canteen;

			ViewData["CanEdit"] = canEdit;


			return View((packet, demoList));
		}



		//CREATE PACKET
		[Authorize(Policy = "Employee")]
		public IActionResult CreatePacket() {
			CanMakeWarmMeals();
			return View();
		}

		[Authorize(Policy = "Employee")]
		[HttpPost]
		public async Task<IActionResult> CreatePacket(string name, string price, DateTime pickupTime, string products, TypeEnum type, string imageUrl) {
			if (!ModelState.IsValid) {
				CanMakeWarmMeals();
				return View();
			}

			await _packetRepository.CreatePacket(name, price, pickupTime, products, type, imageUrl, GetCanteen());
			return RedirectToAction("Packets");
		}


		//EDIT PACKET
		[HttpGet]
		public IActionResult EditPacket(int id) {
			var packet = _packetRepository.GetPacketById(id);
			CanMakeWarmMeals();

			return View(packet);
		}

		[HttpPost]
		public async Task<IActionResult> EditPacket(int id, string name, string price, DateTime pickupTime, string products, TypeEnum type, string imageUrl) {
			await _packetRepository.UpdatePacket(id, name, price, pickupTime, products, type, imageUrl);

			return RedirectToAction("Packets");
		}


		//DELETE PACKET
		[Authorize(Policy = "Employee")]
		[HttpPost]
		public async Task<IActionResult> DeletePacket(int id) {
			var packet = _packetRepository.GetPacketById(id);

			await _packetRepository.DeletePacket(packet);
			return RedirectToAction("Packets");
		}




		//OTHER METHODS
		public Canteen GetCanteen() {
			var email = HttpContext.Session.GetString("UserEmail");
			var canteenId = _employeeRepository.GetEmployees().FirstOrDefault(a => a.Email == email).CanteenId; ;
			return _canteenRepository.GetCanteenById(canteenId);

		}

		private void CanMakeWarmMeals() {
			var canteen = GetCanteen();
			if (canteen.OffersHotMeals) {
				ViewData["WarmMeals"] = true;
			}
			else {
				ViewData["WarmMeals"] = false;
			}
		}


	}

}