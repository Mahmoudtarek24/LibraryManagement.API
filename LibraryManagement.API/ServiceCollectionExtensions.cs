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
			return service;
		}
	}
}
