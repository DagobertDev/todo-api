using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TodoApi.Data;
using TodoApi.Models;

namespace TodoApi.Controllers
{
	[ApiController]
	[Route("users")]
	public class UserController : ControllerBase
	{
		private readonly IUserService _userService;

		public UserController(IUserService userService)
		{
			_userService = userService;
		}

		[HttpPost]
		public async Task<ActionResult<string>> RegisterAsync(RegisterModel model)
		{
			var result = await _userService.RegisterAsync(model);

			if (!result.Succeeded)
			{
				return BadRequest(result.Errors.Select(error => error.Description));
			}

			return NoContent();
		}

		[HttpPost("refresh-token")]
		public async Task<ActionResult<AuthenticationResponse>> Authenticate(AuthenticationRequest model)
		{
			var response = await _userService.AuthenticateAsync(model);

			if (response == null)
			{
				return BadRequest(new { message = "Username or password is incorrect" });
			}

			SetRefreshTokenCookie(response.RefreshToken);

			return response;
		}

		[HttpDelete("refresh-token")]
		public async Task<IActionResult> Logout()
		{
			var token = Request.Cookies["refreshToken"];
			Response.Cookies.Delete("refreshToken");
			return await RevokeToken(token);
		}

		[HttpDelete("refresh-token/{token}")]
		public async Task<IActionResult> RevokeToken(string token)
		{
			if (string.IsNullOrEmpty(token))
			{
				return BadRequest(new { message = "Token is required" });
			}

			var response = await _userService.RevokeToken(token);

			if (!response)
			{
				return NotFound(new { message = "Token not found" });
			}

			return NoContent();
		}

		[HttpGet("access-token")]
		public async Task<ActionResult<AuthenticationResponse>> RefreshAccessToken()
		{
			var refreshToken = Request.Cookies["refreshToken"];
			var response = await _userService.RefreshAccessTokenAsync(refreshToken);

			if (response == null)
			{
				return BadRequest(new { message = "Invalid token" });
			}

			SetRefreshTokenCookie(response.RefreshToken);

			return response;
		}

		private void SetRefreshTokenCookie(RefreshToken token)
		{
			var cookieOptions = new CookieOptions
			{
				HttpOnly = true,
				Expires = token.Expires
			};
			Response.Cookies.Append("refreshToken", token.Token, cookieOptions);
		}
	}
}
