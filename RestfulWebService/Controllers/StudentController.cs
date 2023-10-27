using Domain.Models;
using Domain.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestfulWebService.Models;

namespace RestfulWebService.Controllers {

	[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
	[Route("/[controller]")]
	[ApiController]

	public class StudentController : ControllerBase {

		private readonly IPacketRepository _packetRepository;
		private readonly IStudentRepository _studentRepository;


		public StudentController(IPacketRepository packetRepository, IStudentRepository studentRepository) {
			_packetRepository = packetRepository;
			_studentRepository = studentRepository;
			
		}

		[Authorize(Policy = "Employee")]
		[HttpGet]
		public ActionResult<List<Student>> Get() {
			return Ok(_studentRepository.GetStudents());
		}


		[Authorize(Policy = "Student")]
		[HttpPost("Reservation")]
		public async Task<IActionResult> ReservePacket([FromBody] ReservationRequest request) {
			try {
				if (request.studentId == 0 || request.packetId == 0) {
					return BadRequest("Invalid reservation request.");
				}

				var result = await _packetRepository.ReservePacketBool(request.packetId, request.studentId);

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

