using System.ComponentModel.DataAnnotations;

namespace RestfulWebService.Models {
	public class AuthenticationCredentials {
		[Required]
		public string Email { get; set; }

		[Required]
		public string Password { get; set; }

	}
}
