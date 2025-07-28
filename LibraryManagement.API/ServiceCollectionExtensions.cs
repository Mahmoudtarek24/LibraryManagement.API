using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Security.Claims;
using System.Text;
using System.Text.Json.Serialization;

namespace LibraryManagement
{
	public static class ServiceCollectionExtensions
	{
		public static IServiceCollection AddApiServices(this IServiceCollection service, IConfiguration configuration)
		{
			service.AddControllers().AddJsonOptions(opthions =>
			{
				opthions.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
			});

			service.AddSwaggerGen(options =>
			{
				//options.SwaggerDoc("v1", new OpenApiInfo
				//{
				//	Version = "v1",
				//	Title = "Inventory Management",
				//	Contact = new OpenApiContact
				//	{
				//		Name = "Mahmoud Tarek",
				//		Email = "mahmoudtark556@gmail.com"
				//	},
				//	Description = "Inventory System API - Learning Project",
				//	License = new OpenApiLicense
				//	{
				//		Name = "View on GitHub",
				//		Url = new Uri("")
				//	}
				//});
				options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
				{
					Name = "Authorization",
					Type = SecuritySchemeType.ApiKey,
					Scheme = "Bearer",
					BearerFormat = "JWT",
					In = ParameterLocation.Header,

				});
				options.AddSecurityRequirement(new OpenApiSecurityRequirement
				{
					{
						new OpenApiSecurityScheme
						{
							Reference=new OpenApiReference
							{
								Type=ReferenceType.SecurityScheme,
								Id="Bearer"
							},
							Name="Bearer",
							In=ParameterLocation.Header
						},
						new List<string>(){}
					}
				});

			});

			service.AddAuthentication(opthios =>
			{
				opthios.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				opthios.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
				opthios.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
			}).AddJwtBearer(options =>
			{
				options.SaveToken = true;
				options.RequireHttpsMetadata = false;
				options.TokenValidationParameters = new TokenValidationParameters
				{
					ValidateIssuer = true,
					ValidateAudience = true,
					ValidateIssuerSigningKey = true,
					ValidateLifetime = true,
					ValidIssuer = configuration["JWTSetting:Issuer"],
					ValidAudience = configuration["JWTSetting:Audience"],
					IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWTSetting:Key"])),
					ClockSkew = TimeSpan.Zero,
					RoleClaimType = ClaimTypes.Role
				};
			});
			service.AddAuthorization();

			return service;
		}
	}
}