using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Models;
using Domain.Models.Enums;
using Domain.Services;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using UI.Controllers;

namespace UI.Tests.Controllers {
    public class StudentControllerTests {
        private readonly StudentController _studentController;
        private readonly IPacketRepository _packetRepository = Substitute.For<IPacketRepository>();
        private readonly IStudentRepository _studentRepository = Substitute.For<IStudentRepository>();
        private readonly IDemoProductRepository _demoProductRepository = Substitute.For<IDemoProductRepository>();


        public StudentControllerTests() {
            _studentController = new StudentController(_packetRepository, _studentRepository, _demoProductRepository);
        }


        //[Fact] 
        //public void Packets_ReturnsSuccess() {
        //    // Arrange
        //    var packets = GetPacketList();
        //    _packetRepository.GetPackets().Returns(packets); 
        //    // Act
        //    var result = _studentController.Packets();

        //    // Assert
           

        //}


        [Fact]
        public void Packets_ReturnsViewResult() {
            // Arrange


            // Act
            var result = _studentController.Packets() as ViewResult;

            // Assert
            Assert.IsType<ViewResult>(result);
            

        }

        //[Fact]
        //public void Packets_ContainsPackets_ReturnsData() {
        //    // Arrange


        //    // Act
        //    var result = _studentController.Packets() as ViewResult;
        //    var packets = (Packet)result.ViewData.Model;


        //    // Assert

        //}


        //[Fact]
        //public void Packets_NoPackets_ReturnsViewResult() {
        //    // Arrange

        //}

        //[Fact]
        //public void Packets_AllPackets_ReturnsViewResult() {

        //    // Act
        //    var result = _studentController.Packets();

        //    // Assert
        //    var viewResult = Assert.IsType<ViewResult>(result);
        //    var model = Assert.IsType<List<Packet>>(viewResult.Model);

        //    // Now you can work with the model, which is a List<Packet>
        //    Assert.NotNull(model); // Ensure the model is not null
        //                           // Add additional assertions based on the specific scenario

     //   }

        [Fact]
        public void PacketDetails_WithValidId_ReturnsViewResult() {
            // Arrange


            // Act
            var result = _studentController.PacketDetails(1);

            // Assert
            Assert.IsType<ViewResult>(result);
        }

        //[Fact]
        //public void PacketDetails_WithInvalidId_ReturnsViewResult() {
        //    // Arrange
        //    _packetRepository.GetPacketById(1).Returns((Packet)null); // Simulate not finding the packet


        //    // Act
        //    var result = _studentController.PacketDetails(1);

        //    // Assert
        //    Assert.IsType<ViewResult>(result);
        //}



        public IEnumerable<Packet> GetPacketList() {


            var packets = new List<Packet> {

            new Packet { Name = "Lekkere Broodjes" , City = CityEnum.Breda, Canteen = CanteenEnum.LA, DateTime = DateTime.Now, Price = 8.99m, Type = TypeEnum.Broodpakket, ReservedById = null, ImageUrl = "https://rosco-catering.nl/wp-content/uploads/2020/06/Rosco-Catering-Bake-off-box-scaled.jpg" },
            new Packet { Name = "Drankpakket", City = CityEnum.Breda, Canteen = CanteenEnum.LA, DateTime = DateTime.Now, Price = 14.99m, Type = TypeEnum.Drankpakket, ReservedById = null, ImageUrl = "https://www.foodandwine.com/thmb/a3jODP_x_GpJpD71zT3t3BYbtp8=/1500x0/filters:no_upscale():max_bytes(150000):strip_icc()/how-to-ship-wine-FT-BLOG1221-073f4b1897c34f04bff8ea71dadcba2c.jpg" },
            new Packet { Name = "Warme Maaltijd", City = CityEnum.Tilburg, Canteen = CanteenEnum.HA, DateTime = DateTime.Now, Price = 22.99m, Type = TypeEnum.WarmeMaaltijd, ReservedById = null, ImageUrl = "https://www.foodandwine.com/thmb/a3jODP_x_GpJpD71zT3t3BYbtp8=/1500x0/filters:no_upscale():max_bytes(150000):strip_icc()/how-to-ship-wine-FT-BLOG1221-073f4b1897c34f04bff8ea71dadcba2c.jpg" }

        };

            return packets;

        }
    }
}
