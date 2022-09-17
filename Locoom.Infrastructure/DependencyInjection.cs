using Locoom.Application.Common.Interfaces.Authentication;
using Locoom.Application.Common.Interfaces.Services;
using Locoom.Infrastructure.Authentication;
using Locoom.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Locoom.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            ConfigurationManager configuration)
        {
            services.Configure<JwtSettings>(configuration.GetSection(JwtSettings.SectionName));

            services.AddSingleton<IJwtTokenGenerator, JwtTokenGenerator>();
            services.AddSingleton<IDatetimeProvider, DateTimeProvider>();

            return services;
        }
    }
}
