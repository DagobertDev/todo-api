using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using TodoApi.Models;
using TodoApi.Options;

namespace TodoApi.Data
{
	public class UserService : IUserService
	{
		private readonly JWT _jwt;
		private readonly UserManager<ApplicationUser> _userManager;

		public UserService(UserManager<ApplicationUser> userManager, IOptions<JWT> jwt)
		{
			_userManager = userManager;
			_jwt = jwt.Value;
		}

		public async Task<(string, IdentityResult)> RegisterAsync(RegisterModel model)
		{
			var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
			var result = await _userManager.CreateAsync(user, model.Password);
			return (result.Succeeded ? user.Id : null, result);
		}

		public async Task<AuthenticationModel> GetTokenAsync(TokenRequestModel model)
		{
			var authenticationModel = new AuthenticationModel();
			var user = await _userManager.FindByEmailAsync(model.Email);

			if (user == null)
			{
				return authenticationModel;
			}

			if (!await _userManager.CheckPasswordAsync(user, model.Password))
			{
				return authenticationModel;
			}

			authenticationModel.IsAuthenticated = true;
			var validUntil = DateTime.UtcNow.AddMinutes(_jwt.DurationInMinutes);
			authenticationModel.ValidUntil = validUntil;

			var jwtSecurityToken = await CreateJwtToken(user, validUntil);
			authenticationModel.Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
			var rolesList = await _userManager.GetRolesAsync(user).ConfigureAwait(false);
			authenticationModel.Roles = rolesList;

			return authenticationModel;
		}

		private async Task<JwtSecurityToken> CreateJwtToken(ApplicationUser user, DateTime expires)
		{
			var userClaims = await _userManager.GetClaimsAsync(user);
			var roles = await _userManager.GetRolesAsync(user);
			var roleClaims = roles.Select(t => new Claim("roles", t)).ToList();

			var claims = new[]
			{
				new Claim(JwtRegisteredClaimNames.Sub, user.Id),
				new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
				new Claim(JwtRegisteredClaimNames.Email, user.Email),
				new Claim("uid", user.Id)
			}.Union(userClaims).Union(roleClaims);

			var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
			var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);
			var jwtSecurityToken = new JwtSecurityToken(_jwt.Issuer, _jwt.Audience, claims,
				expires: expires, signingCredentials: signingCredentials);
			return jwtSecurityToken;
		}
	}
}
