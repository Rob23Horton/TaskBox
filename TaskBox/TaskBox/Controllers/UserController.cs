using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TaskBox.Interfaces;
using TaskBox.Shared.Models;

namespace TaskBox.Controllers
{
	[ApiController]
	[Route("api/User")]
	public class UserController : Controller
	{
		public readonly IUserRepository userRepository;
		public readonly HttpContent httpContent;
		public UserController(IUserRepository userRepository)
		{
			this.userRepository = userRepository;
		}

		[HttpGet("LogIn")]
		[ProducesResponseType(200, Type=typeof(bool))]
		[ProducesResponseType(400, Type=typeof(bool))]
		public async Task<IActionResult> CheckUserCredentials(string UserName, string Password)
		{
			try
			{
				User? user = userRepository.CheckUserCredentials(UserName, Password);

				if (user is null)
				{
					await HttpContext.SignOutAsync();

					return Ok(false);
				}

				var claims = new List<Claim>{
					new Claim(ClaimTypes.Sid, user.Id.ToString()),
					new Claim(ClaimTypes.Name, user.UserName)
				};

				var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
				var principal = new ClaimsPrincipal(identity);
				await HttpContext.SignInAsync(principal);

				return Ok(true);
			}
			catch
			{
				return BadRequest(false);
			}
		}

		[HttpGet("LogOut")]
		public async Task<IActionResult> LogOut()
		{
			try
			{
				await HttpContext.SignOutAsync();

				return Ok();
			}
			catch
			{
				return BadRequest();
			}
		}
	}
}
