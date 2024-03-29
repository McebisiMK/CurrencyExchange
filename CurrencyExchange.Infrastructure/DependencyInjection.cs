using CurrencyExchange.Application.Common.Interfaces;
using CurrencyExchange.Domain.Models;
using CurrencyExchange.Infrastructure.Repositories;
using CurrencyExchange.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CurrencyExchange.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection RegisterInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("CurrencyExchangeDatabase");

            services.AddScoped<ICacheService, CacheService>();
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

            services.AddDbContext<CurrencyExchangeContext>(options =>
            {
                options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
            });

            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = configuration.GetConnectionString("RedisConnectionString");
            });

            services.AddHttpClient<ICurrencyExchangeService, CurrencyExchangeService>()
                    .ConfigureHttpClient(httpClient =>
                    {
                        var baseUrl = configuration.GetValue<string>("CurrencyExchangeAPIOptions:BaseUrl");
                        httpClient.BaseAddress = new Uri(baseUrl);
                    });

            return services;
        }
    }
}
