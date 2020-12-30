using System;
using System.Collections.Generic;

namespace TodoApi.Models
{
	public class AuthenticationModel
	{
		public bool IsAuthenticated { get; set; }
		public IList<string> Roles { get; set; }
		public DateTime ValidUntil { get; set; }
		public string Token { get; set; }
	}
}
