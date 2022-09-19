using Locoom.API.Common.Errors;
using Locoom.API.Common.Mapping;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace Locoom.API
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPresentation(this IServiceCollection services)
        {

            services.AddControllers();

            services.AddSingleton<ProblemDetailsFactory, LocoomProblemDetailsFactory>();

            services.AddMapping();

            return services;
        }
    }
}
