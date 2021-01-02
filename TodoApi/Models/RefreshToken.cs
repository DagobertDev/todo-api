using System;
using Microsoft.EntityFrameworkCore;

namespace TodoApi.Models
{
	[Owned]
	public class RefreshToken
	{
		public string Token { get; set; }
		public DateTime Expires { get; set; }
		public bool IsExpired => DateTime.UtcNow >= Expires;
		public DateTime Created { get; set; }
		public bool IsActive => !IsExpired;
	}
}
