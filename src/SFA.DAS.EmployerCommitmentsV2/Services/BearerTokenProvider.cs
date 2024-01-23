using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace SFA.DAS.EmployerCommitmentsV2.Services
{
	public static class BearerTokenProvider
	{
		private static string _signingKey = string.Empty;
		private const int _expiryTimeMinutes = 5;

		/// <summary>
		/// Set the bearer token signing key, must exceed 128bits in length
		/// </summary>
		public static void SetSigningKey(string signingKey)
		{
			if (string.IsNullOrEmpty(signingKey))
			{
				throw new BearerTokenException("Signing key cannot be null or empty");
			}

			const int minimumKeySize = 128;
			if (signingKey.Length * 8 < minimumKeySize)
			{
				// This checks the key is at least 128 bits long, otherwise the token will fail to be generated
				throw new BearerTokenException("Signing key must be at least 128bits in length");
			}

			_signingKey = signingKey;
		}

		public static bool TryGetBearerToken(this HttpContext httpContext, out string bearerToken)
		{
			if (string.IsNullOrEmpty(_signingKey))
			{
				throw new BearerTokenException("Signing key must be set before a token can be retrieved. This should ideally be done in startup");
			}

			var user = httpContext.User;
			if (!IsUserAuthenticated(user))
			{
				bearerToken = string.Empty;
				return false;
			}

			var key = new SymmetricSecurityKey(System.Text.Encoding.ASCII.GetBytes(_signingKey));
			var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

			var token = new JwtSecurityToken(
				claims: user.Claims,
				signingCredentials: creds,
				expires: DateTime.UtcNow.AddMinutes(_expiryTimeMinutes)
			);

			bearerToken = new JwtSecurityTokenHandler().WriteToken(token);
			return true;
		}

		private static bool IsUserAuthenticated(ClaimsPrincipal user)
		{
			if (user == null) return false;

			if (user.Identity == null) return false;

			return user.Identity.IsAuthenticated;
		}
	}

	public class BearerTokenException : Exception
	{
		public BearerTokenException(string message) : base(message)
		{
		}
	}
}
