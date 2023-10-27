using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain {
	public class SignInResponse {
		public bool Success { get; set; }

		public string Token { get; set; } = string.Empty;
	}
}
