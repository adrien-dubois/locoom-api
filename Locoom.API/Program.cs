using Locoom.API;
using Locoom.Application;
using Locoom.Infrastructure;
using Locoom.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);
{
    builder.Services
        .AddPresentation()
        .AddApplication()
        .AddInfrastructure(builder.Configuration);

    builder.Services.AddControllers().AddJsonOptions(x =>
    {
        x.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

    builder.Services.AddDbContextPool<DatabaseContext>(opt =>
    {
        var cs = builder.Configuration.GetConnectionString("LocoomDatabase");
        opt.UseMySql(cs, ServerVersion.AutoDetect(cs), b => b.MigrationsAssembly("Locoom.API"));
    });
}

var app = builder.Build();
{
    app.UseExceptionHandler("/error");

    app.UseAuthentication();
    app.UseAuthorization();
    app.MapControllers();
    app.Run();
}
 