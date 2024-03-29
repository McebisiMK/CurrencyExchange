using System.Reflection;
using CurrencyExchange.Application.Helpers;
using Microsoft.Extensions.DependencyInjection;
using FluentValidation.AspNetCore;

namespace CurrencyExchange.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection RegisterApplication(this IServiceCollection services)
        {
            services.AddTransient<IValidatorInterceptor, RequestValidatorInterceptor>();
            services.AddFluentValidation(config =>
            {
                config.RegisterValidatorsFromAssembly(Assembly.GetExecutingAssembly());
                config.AutomaticValidationEnabled = true;
            });
            services.AddMediatR(config => config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            return services;
        }
    }
}
