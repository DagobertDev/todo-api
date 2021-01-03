using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using TodoApi.Models;

namespace TodoApi.Data
{
	public interface IUserService
	{
		Task<IdentityResult> RegisterAsync(RegisterModel model);
		Task<AuthenticationResponse> AuthenticateAsync(AuthenticationRequest model);
		Task<AuthenticationResponse> RefreshAccessTokenAsync(string refreshToken);
		Task<bool> RevokeToken(string token);
	}
}
