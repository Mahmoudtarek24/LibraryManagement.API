using Application.Interfaces;
using Application.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application
{
	public static class ServiceCollectionExtensions
	{
		public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddScoped<IAuthorService,AuthorService>();
			services.AddScoped<IBookService, BookService>();
			services.AddScoped<IUserService, UserService>();
			services.AddScoped<IBorrowingService, BorrowingService>();
			return services;
		}
	}
}
