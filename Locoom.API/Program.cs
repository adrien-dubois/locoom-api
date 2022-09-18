using Locoom.API.Errors;
using Locoom.API.Filters;
using Locoom.Application;
using Locoom.Infrastructure;
using Locoom.Infrastructure.Persistance;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
{
    builder.Services
        .AddApplication()
        .AddInfrastructure(builder.Configuration);

    // builder.Services.AddControllers(options => options.Filters.Add<ErrorHandlingFilterAttribute>());
    builder.Services.AddControllers();

    builder.Services.AddSingleton<ProblemDetailsFactory, LocoomProblemDetailsFactory>();

    builder.Services.AddDbContextPool<DatabaseContext>(opt =>
    {
        var cs = builder.Configuration.GetConnectionString("LocoomDatabase");
        opt.UseMySql(cs, ServerVersion.AutoDetect(cs), b => b.MigrationsAssembly("Locoom.API"));
    });
}

var app = builder.Build();
{
    // app.UseMiddleware<ErrorHandlingMiddleware>();
    app.UseExceptionHandler("/error");

    app.Map("/error", (HttpContext httpContext) =>
    {
        Exception? exception = httpContext.Features.Get<IExceptionHandlerFeature>()?.Error;

        return Results.Problem();
    });

    app.MapControllers();
    app.Run();
}
