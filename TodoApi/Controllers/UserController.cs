using System.Linq;
using System.Threading.Tasks;
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
			var (id, result) = await _userService.RegisterAsync(model);

			if (!result.Succeeded)
			{
				return BadRequest(result.Errors.Select(error => error.Description));
			}

			return CreatedAtAction("Register", model, id);
		}

		[HttpGet("token")]
		public async Task<ActionResult<AuthenticationModel>> GetTokenAsync(TokenRequestModel model)
		{
			var result = await _userService.GetTokenAsync(model);
			return result;
		}
	}
}
