﻿using Domain.Models;
using Domain.Models.Enums;
using Microsoft.AspNetCore.Mvc;
using Infrastructure.Repositories;
using Domain.Services;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using Newtonsoft.Json;
using Infrastructure.Migrations.AppDB;
using Microsoft.IdentityModel.Tokens;

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
            //_packetRepository getPacketDetails();
            
            var packet = _packetRepository.GetPacketById(id);
            var demoList = _demoProductRepository.GetDemoProducts(packet.Type);


            var canteen = GetCanteen();


            bool canEdit = packet.CanteenNavigation == canteen; // Adjust this condition as needed
            ViewData["CanEdit"] = canEdit;

            if (packet == null) {
                return View("Packets");
            }
            return View((packet, demoList));
        }


        [Authorize(Policy = "Employee")]
        public IActionResult CreatePacket() {
            var canteen = GetCanteen();


            if (canteen.OffersHotMeals == true) {
                ViewData["WarmMeals"] = true;
            }
            else {
                ViewData["WarmMeals"] = false;
            }
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


        [HttpGet]
        public IActionResult EditPacket(int id) {
            // Fetch the packet by id and pass it to the edit view
            var packet = _packetRepository.GetPacketById(id);
            if (packet == null) {
                return NotFound(); // Handle not found packet
            }

            CanMakeWarmMeals();

            return View(packet);
        }

        [HttpPost]
        public async Task<IActionResult> EditPacket(int id, string name, string price, DateTime pickupTime, string products, TypeEnum type, string imageUrl) {
            // Validate and update the packet in the database
            var test = products;
           
            //List<Product> productObjects = new List<Product>();
           // Packet packet = new Packet(name, pickupTime, productObjects, price, type, imageUrl);
            await _packetRepository.UpdatePacket(id,  name,  price,  pickupTime,  products,  type,  imageUrl);
         
            return RedirectToAction("Packets");



         
        }



        [Authorize(Policy = "Employee")]
        [HttpPost]
        public async Task<IActionResult> DeletePacket(int id) {
            var packet = _packetRepository.GetPacketById(id);

            await _packetRepository.DeletePacket(packet);
            return RedirectToAction("Packets");
        }


        [Authorize(Policy = "Employee")]
        public IActionResult Packets() {        
            var packetsOurs2 = _packetRepository.GetMyCanteenPackets(GetCanteen());
            var packetsOthers2 = _packetRepository.GetOtherCanteenPackets(GetCanteen());

            return View((packetsOurs2, packetsOthers2));
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