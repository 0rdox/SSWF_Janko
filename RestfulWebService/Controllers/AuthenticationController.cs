using System.IdentityModel.Tokens.Jwt;
using System.Runtime.CompilerServices;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using RestfulWebService.Models;

namespace RestfulWebService.Controllers {
	public class AuthenticationController : ControllerBase {
		private readonly UserManager<IdentityUser> _userMgr;
		private readonly SignInManager<IdentityUser> _signInMgr;

		public AuthenticationController(UserManager<IdentityUser> userMgr, SignInManager<IdentityUser> signInMgr) {
			_userMgr = userMgr;
			_signInMgr = signInMgr;
		}


		[HttpPost("api/signin")]
		public async Task<IActionResult> SignIn([FromBody] AuthenticationCredentials authenticationCredentials) {
			var user = new IdentityUser();
			try {
				user = await _userMgr.FindByEmailAsync(authenticationCredentials.Email);
			} catch(Exception ex) {

			}
			if (user != null) {
				if ((await _signInMgr.PasswordSignInAsync(user,
					authenticationCredentials.Password, false, false)).Succeeded) {
					var securityTokenDescriptor = new SecurityTokenDescriptor {
						Subject = (await _signInMgr.CreateUserPrincipalAsync(user)).Identities.First(),
						Expires = DateTime.Now.AddMinutes(60),
						SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes("SecretKeyVeryLong")), SecurityAlgorithms.HmacSha256Signature)
					};

					

					var handler = new JwtSecurityTokenHandler();
					var securityToken = new JwtSecurityTokenHandler().CreateToken(securityTokenDescriptor);

					return Ok(new { Succes = true, Token = handler.WriteToken(securityToken) });
				}
			}

			return BadRequest();
		}


		[HttpPost("api/signout")]
		public async Task<IActionResult> SignOut() {
			await _signInMgr.SignOutAsync();
			return Ok();
		}
	}
}
