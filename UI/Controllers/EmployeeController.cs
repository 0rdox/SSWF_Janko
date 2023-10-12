using Domain.Models;
using Domain.Models.Enums;
using Microsoft.AspNetCore.Mvc;
using Infrastructure.Repositories;
using Domain.Services;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using Newtonsoft.Json;

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

        public IActionResult Index() {
            return View();
        }


        //PACKET DETAILS
        [Authorize(Policy = "Employee")]
        public IActionResult PacketDetails(int id) {
            var packet = _packetRepository.GetPacketById(id);
            var demoList = _demoProductRepository.GetDemoProducts(packet.Type);


            if (packet == null) {
                return View("Packets");
            }
            return View((packet, demoList));
        }


        [Authorize(Policy = "Employee")]
        public IActionResult CreatePacket() {
            return View();
        }

        [Authorize(Policy = "Employee")]
        [HttpPost]
        public async Task<IActionResult> CreatePacket(string name, decimal price, DateTime pickupTime, string products, TypeEnum type, string imageUrl) {
            //Get location for packet
            var canteenLocation = _canteenRepository.GetCanteenById(GetCanteenID()).Location;
            var canteenCity = _canteenRepository.GetCanteenById(GetCanteenID()).City;

            List<Product> productObjects = JsonConvert.DeserializeObject<List<Product>>(products);

            //"[{\"Name\":\"Bread\",\"Alcohol\":false,\"ImageUrl\":\"bread.img\"},{\"Name\":\"bread2\",\"Alcohol\":true,\"ImageUrl\":\"breadimg2\"}]"
            Packet packet = new Packet(name, pickupTime, productObjects, price, type, imageUrl);

            DateTime testMaxDateTime = packet.MaxDateTime;
            bool testOverEighteen = packet.OverEighteen;
            //_packetRepository.CreatePacket(packet);

            //check max date time
            //check over eighteen



            return View();
        }



        [Authorize(Policy = "Employee")]
        public IActionResult Packets() {
            var canteen = _canteenRepository.GetCanteenById(GetCanteenID());

            var packetsOurs = _packetRepository.GetPackets()
                .Where(a => a.ReservedById == null)
                .Where(b => b.CanteenNavigation == canteen)
                  .OrderBy(c => c.DateTime);

            var packetsOthers = _packetRepository.GetPackets()
                .Where(a => a.ReservedById == null)
                .Where(b => b.CanteenNavigation != canteen)
                .OrderBy(c => c.DateTime);

            return View((packetsOurs, packetsOthers));
        }

        public int GetCanteenID() {
            var userName = HttpContext.Session.GetString("UserName");
            var email = HttpContext.Session.GetString("UserEmail");
            var id = HttpContext.Session.GetString("Id");
            //test this
            return _employeeRepository.GetEmployees().FirstOrDefault(a => a.Email == email).CanteenId;
        }



    }

}