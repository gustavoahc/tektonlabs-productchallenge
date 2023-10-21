using Microsoft.Extensions.DependencyInjection;
using TektonLabs.Core.Application.Services.Products;
using TektonLabs.Core.Application.Validators;

namespace TektonLabs.Core.Application
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<ProductValidator>();
            return services;
        }
    }
}
