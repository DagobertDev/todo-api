using System;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace TodoApi.Controllers
{
	[ApiController]
	public class ErrorController : ControllerBase
	{
		[Route("/error")]
		public IActionResult Error()
		{
			var error = HttpContext
				.Features
				.Get<IExceptionHandlerFeature>();

			if (error == null)
			{
				return null;
			}

			var exception = error.Error;

			if (exception is ArgumentException)
			{
				return BadRequest(exception.Message);
			}

			return Problem(exception.Message);
		}
	}
}
