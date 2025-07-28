using Application.Constants.Enums;
using Application.Interfaces;
using Domain.Entities;
using Infrastructure.UnitOfWork;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
	public class JwtTokenGenerator : ITokenGenerator
	{
		private readonly IConfiguration config;
		public JwtTokenGenerator(IConfiguration configuration)
		{
			this.config = configuration;
		}
		public async Task<string> GenerateAsync(User user)
		{
			List<Claim> claims = new List<Claim>();

			claims.Add(new Claim(ClaimTypes.Role, user.Role));
			claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));
			claims.Add(new Claim(ClaimTypes.Email, user.Email));
			claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));

			SymmetricSecurityKey symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JWTSetting:Key"]));
			SigningCredentials signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);
			
			var token= new JwtSecurityToken(
				claims: claims,
				issuer: config["JWTSetting:Issuer"],
				audience: config["JWTSetting:Audience"],
				expires: DateTime.Now.AddDays(double.Parse(config["JWTSetting:DurationInDays"])),
				signingCredentials: signingCredentials
			);
		
			return new JwtSecurityTokenHandler().WriteToken(token);
		}
	
	}
}
