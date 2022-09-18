using Locoom.API.Common.Errors;
using Locoom.Application;
using Locoom.Infrastructure;
using Locoom.Infrastructure.Persistance;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
{
    builder.Services
        .AddApplication()
        .AddInfrastructure(builder.Configuration);

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
    app.UseExceptionHandler("/error");

    app.MapControllers();
    app.Run();
}
 