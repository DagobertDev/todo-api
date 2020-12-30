using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using TodoApi.Models;

namespace TodoApi.Data
{
	public interface IUserService
	{
		Task<(string, IdentityResult)> RegisterAsync(RegisterModel model);
		Task<AuthenticationModel> GetTokenAsync(TokenRequestModel model);
	}
}
