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


			var canteen = _canteenRepository.GetCanteenById(GetCanteenID());


			bool canEdit = packet.CanteenNavigation == canteen; // Adjust this condition as needed
            ViewData["CanEdit"] = canEdit;

            if (packet == null) {
                return View("Packets");
            }
            return View((packet, demoList));
        }


        [Authorize(Policy = "Employee")]
        public IActionResult CreatePacket() {
            var canteen = _canteenRepository.GetCanteenById(GetCanteenID());
            if (canteen.OffersHotMeals == true) {
                ViewData["WarmMeals"] = true;
            } else {
                ViewData["WarmMeals"] = false;
            }
            return View();
        }

        [Authorize(Policy = "Employee")]
        [HttpPost]
        public async Task<IActionResult> CreatePacket(string name, decimal price, DateTime pickupTime, string products, TypeEnum type, string imageUrl) {

            //if (!ModelState.IsValid) {
            //    return View();
            //}

            var canteenLocation = _canteenRepository.GetCanteenById(GetCanteenID()).Location;
            var canteenCity = _canteenRepository.GetCanteenById(GetCanteenID()).City;

            List<Product> productObjects = JsonConvert.DeserializeObject<List<Product>>(products);

     

            Packet packet = new Packet(name, pickupTime, productObjects, price, type, imageUrl);

          await  _packetRepository.CreatePacket(packet);



            //redirect to action

            return View("Packets");
        }


        [Authorize(Policy = "Employee")]
        [HttpPost]
        public async Task<IActionResult> DeletePacket(int id) {
			var packet = _packetRepository.GetPacketById(id);

			if (packet == null) {
                return NotFound(); // Packet not found
            }

            // Check if the employee is authorized to delete the packet. You may want to add authorization logic here.

            await _packetRepository.DeletePacket(packet);

            return RedirectToAction("Packets"); 
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