
//using Domain.Services.Seed;
using Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Security.Claims;

namespace UI.Controllers {
	public class AccountController : Controller {

		private readonly UserManager<IdentityUser> _userManager;
		private readonly SignInManager<IdentityUser> _signInManager;


		public AccountController(UserManager<IdentityUser> userMgr, SignInManager<IdentityUser> signInMgr) {
			_userManager = userMgr;
			_signInManager = signInMgr;
		}


		[HttpGet]
		public IActionResult Login() {
			return View();
		}


		[HttpPost]
		public async Task<IActionResult> Login(string email, string password) {
			if (ModelState.IsValid) {
				//SEARCH USER
				var user = await _userManager.FindByEmailAsync(email);
				if (user != null) {
					var result = await _signInManager.PasswordSignInAsync(user, password, false, false);

					if (result.Succeeded) {
						//CHECK STUDENT OR EMPLOYEE
						var role = await _userManager.GetRolesAsync(user);

						//ADD EMAIL + USERNAME TO SESSION
						HttpContext.Session.SetString("UserName", user.UserName);
						HttpContext.Session.SetString("UserEmail", user.Email);
						
						HttpContext.Session.SetString("Id", user.Id);

						switch (role[0]) {
							case "Employee":
								return RedirectToAction("Index", "Employee");

							case "Student":
								return RedirectToAction("Index", "Student");

						};

						return View();
					}
				}
			}
			ModelState.AddModelError("", "Invalid name or password");
			return View();
		}

		public IActionResult Account() {
			return View();
		}


		public async Task<IActionResult> Logout() {
			await _signInManager.SignOutAsync();
			return View("Login");
		}

        public IActionResult AccessDenied() {
            return View();
        }




		


	}
}
