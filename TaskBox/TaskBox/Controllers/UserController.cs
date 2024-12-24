using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TaskBox.Interfaces;
using TaskBox.Shared.Models;
using Microsoft.AspNetCore.Identity;

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

		[HttpPost("LogIn")]
		[ProducesResponseType(200, Type=typeof(bool))]
		[ProducesResponseType(400, Type=typeof(bool))]
		public async Task<IActionResult> CheckUserCredentials(UserLogin loginAttempt)
		{
			try
			{
				User? user = userRepository.CheckUserCredentials(loginAttempt.UserName, loginAttempt.Password);

				if (user is null)
				{
					await HttpContext.SignOutAsync();

					return Ok(false);
				}

				var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);

				identity.AddClaim(new Claim(ClaimTypes.Sid, user.Id.ToString()));
				identity.AddClaim(new Claim(ClaimTypes.Name, user.UserName));
				identity.AddClaim(new Claim(ClaimTypes.Role, "User"));

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
