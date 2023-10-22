using Domain.Models;
using Domain.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace RestfulWebService.Controllers {

    [Route("/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase {

        private readonly IPacketRepository _packetRepository;
        private readonly IStudentRepository _studentRepository;
        private readonly IDemoProductRepository _demoProductRepository;


        public StudentController(IPacketRepository packetRepository, IStudentRepository studentRepository, IDemoProductRepository demoProductRepository) {
            _packetRepository = packetRepository;
            _studentRepository = studentRepository;
            _demoProductRepository = demoProductRepository;
        }

        [HttpGet]
        public ActionResult<List<Student>> Get() { 
        return Ok(_studentRepository.GetStudents());
        }

        [HttpPost("{studentId}/reserve/{packetId}")]
        public async Task<IActionResult> ReservePacket(int studentId, int packetId) {
            try {
                if (studentId == 0 || packetId == 0) {
                    return BadRequest("Invalid reservation request.");
                }

                var result = await _packetRepository.ReservePacketBool(packetId, studentId);

                if (result) {
                    return Ok("Packet reserved successfully");
                }
                else {
                    return BadRequest("Failed to reserve packet.");
                }
            }
            catch (Exception ex) {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal Server Error: {ex.Message}");
            }
        }

    }
}

