using System;
using System.Text.Json.Serialization;

namespace TodoApi.Models
{
	public class AuthenticationResponse
	{
		public string Id { get; set; }
		public string Username { get; set; }
		public string JwtToken { get; set; }
		public DateTime Expires { get; set; }

		[JsonIgnore]
		public RefreshToken RefreshToken { get; set; }

		public AuthenticationResponse(ApplicationUser user, string jwtToken, RefreshToken refreshToken)
		{
			Id = user.Id;
			Username = user.UserName;
			JwtToken = jwtToken;
			RefreshToken = refreshToken;
		}
	}
}
