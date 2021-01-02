using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using TodoApi.Models;
using TodoApi.Options;

namespace TodoApi.Data
{
	public class UserService : IUserService
	{
		private readonly Jwt _jwt;
		private readonly UserManager<ApplicationUser> _userManager;

		public UserService(UserManager<ApplicationUser> userManager, IOptions<Jwt> jwt)
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

		public async Task<AuthenticationResponse> AuthenticateAsync(AuthenticationRequest model)
		{
			var user = await _userManager.FindByEmailAsync(model.Email);

			if (user == null || !await _userManager.CheckPasswordAsync(user, model.Password))
			{
				return null;
			}

			var jwtToken = GenerateJwtToken(user);
			var refreshToken = GenerateRefreshToken();

			user.RefreshTokens.Add(refreshToken);
			await _userManager.UpdateAsync(user);

			return new AuthenticationResponse(user, jwtToken, refreshToken);
		}

		public async Task<AuthenticationResponse> RefreshAccessTokenAsync(string token)
		{
			var user = await _userManager.Users.SingleOrDefaultAsync(u => u.RefreshTokens.Any(t => t.Token == token));

			if (user == null)
			{
				return null;
			}

			var refreshToken = user.RefreshTokens.SingleOrDefault(t => t.Token == token);

			if (refreshToken == null || !refreshToken.IsActive)
			{
				return null;
			}

			refreshToken.Expires = DateTime.UtcNow.AddDays(_jwt.RefreshTokenLifetimeInDays);
			await _userManager.UpdateAsync(user);

			var jwtToken = GenerateJwtToken(user);

			return new AuthenticationResponse(user, jwtToken, refreshToken);
		}

		public async Task<bool> RevokeToken(string token)
		{
			var user = await _userManager.Users.SingleOrDefaultAsync(u => u.RefreshTokens.Any(t => t.Token == token));

			if (user == null)
			{
				return false;
			}

			var refreshToken = user.RefreshTokens.Single(t => t.Token == token);
			user.RefreshTokens.Remove(refreshToken);
			await _userManager.UpdateAsync(user);

			return true;
		}

		private RefreshToken GenerateRefreshToken()
		{
			using var rngCryptoServiceProvider = new RNGCryptoServiceProvider();
			var randomBytes = new byte[64];
			rngCryptoServiceProvider.GetBytes(randomBytes);

			return new RefreshToken
			{
				Token = Convert.ToBase64String(randomBytes),
				Expires = DateTime.UtcNow.AddDays(_jwt.RefreshTokenLifetimeInDays),
				Created = DateTime.UtcNow
			};
		}

		private string GenerateJwtToken(ApplicationUser user)
		{
			var tokenHandler = new JwtSecurityTokenHandler();
			var key = Encoding.ASCII.GetBytes(_jwt.Key);
			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Issuer = _jwt.Issuer,
				Audience = _jwt.Audience,
				Subject = new ClaimsIdentity(new[] {new Claim(ClaimTypes.Name, user.Id)}),
				Expires = DateTime.UtcNow.AddMinutes(_jwt.DurationInMinutes),
				SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
					SecurityAlgorithms.HmacSha256Signature)
			};
			var token = tokenHandler.CreateToken(tokenDescriptor);
			return tokenHandler.WriteToken(token);
		}
	}
}
