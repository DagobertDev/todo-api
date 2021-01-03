using System.Text.Json.Serialization;

namespace TodoApi.Models
{
	public class AuthenticationResponse
	{
		public UserReadDto User { get; set; }
		public string JwtToken { get; set; }

		[JsonIgnore]
		public RefreshToken RefreshToken { get; set; }

		public AuthenticationResponse(ApplicationUser user, string jwtToken, RefreshToken refreshToken)
		{
			User = UserReadDto.From(user);
			JwtToken = jwtToken;
			RefreshToken = refreshToken;
		}
	}
}
