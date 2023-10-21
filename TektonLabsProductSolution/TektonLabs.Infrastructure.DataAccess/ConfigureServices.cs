using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TektonLabs.Core.Application.Services.Products;
using TektonLabs.Infrastructure.DataAccess.Persistence;

namespace TektonLabs.Infrastructure.DataAccess
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddDataAccessServices(this IServiceCollection services, ConfigurationManager configuration)
        {
            services.AddDbContext<ProductDbContext>(
                options => options.UseSqlServer(configuration.GetConnectionString("ProductsDatabase"))
            );

            services.AddScoped<IProductRepository, ProductRepository>();
            return services;
        }
    }
}
