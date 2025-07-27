using Domain.Interface;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
	public static class ServiceCollectionExtensions
	{
		public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration) 
		{
			services.AddDbContext<LibraryDbContext>(options =>
			{
				options.UseSqlServer(configuration.GetConnectionString("LibraryDbConnection"));
			});
			services.AddScoped<IUnitOfWork, UnitOfWork.UnitOfWork>();
			return services;	
		}
	}
}
