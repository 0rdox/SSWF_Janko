
using Domain.Models;
using Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Data;
using System.Runtime.Intrinsics.X86;
using System.Security.Claims;

namespace Infrastructure.Data {
	public class RoleAssigner {
		private readonly UserManager<IdentityUser> _userManager;
		private readonly RoleManager<IdentityRole> _roleManager;
		private readonly AppDBContext _context;
		private readonly string password = "Secret123";


		public RoleAssigner(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, AppDBContext context) {
			_userManager = userManager;
			_roleManager = roleManager;
			_context = context;
		}


		public async Task AssignRolesToStudentsAndEmployees() {
			await EnsureRoleExists("Student");


			await EnsureRoleExists("Employee");

			foreach (var student in _context.Students) {
				await CreateUserWithRole(student.Name, student.Email, "Student");
			}


			foreach (var employee in _context.Employees) {
				await CreateUserWithRole(employee.Name, employee.Email, "Employee");
			}


		}

		private async Task EnsureRoleExists(string roleName) {
			if (!await _roleManager.RoleExistsAsync(roleName)) {
				var role = new IdentityRole {
					Name = roleName,
					NormalizedName = roleName.ToUpper()
				};
				await _roleManager.CreateAsync(role);
			}
		}

		private async Task CreateUserWithRole(string userName, string email, string roleName) {
			var user = new IdentityUser {
				UserName = userName.Replace(" ", ""),
				Email = email
			};

			var result = await _userManager.CreateAsync(user, password);
			if (result.Succeeded) {
				await AssignRoleToUser(user, roleName);
			}
		}

		private async Task AssignRoleToUser(IdentityUser user, string roleName) {
			if (!await _userManager.IsInRoleAsync(user, roleName)) {
				await _userManager.AddToRoleAsync(user, roleName);
			}
		}


	}
}
