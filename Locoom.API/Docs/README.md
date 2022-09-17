# .NET 6

## JWT Token Generator:

*__Infrastructure layer__*
-  JwtSettings
-  JwtTokenGenerator
--> Implement in Dependency injection

*__Application Layer__*
-  IJwtTokenGenerator
-  AuthenticationService
-  IAuthenticationService
-  AuthenticationResult

--->> Goto the AuthenticationController in API Layer

Add secret in Project.API/appsettings.json
```json
"JwtSettings": {
    "Secret": "",
    "ExpiryMinutes": 0,
    "Issuer": "",
    "Audience":  ""
  }
```

To access the secret, inject in Program.cs, add builder.Configuration to AddInfrastructure services. And in dependencyInjection of Infrastructure, 
install `Microsoft.Extensions.Options.ConfigurationExtensions` & `Microsoft.Extensions.Options`,
add `ConfigurationManager configuration` to the interface dependencies, and to finish add a new services:

```c#
services.Configure<JwtSettings>(configuration.GetSection(JwtSettings.SectionName));
```

And add into JwtSettings' class this constante : `public string const SectionName = "JwtSettings"`

*(JwtSettings)* is what is configured in the appsettings.json

After that, give the JwtSettings parameters to `JwtTokenGenerator`

Then initialize the user secret to the API project : `dotnet user-secrets init --project .\Project.API`
Set the JwtSettings secret's index first, and second, the value `dotnet user-secrets set --project .\Project.API "JwtSettings:Secret" "super-secret-key-white-umbrella-dev"`

Youcan get the user-secrets' list with `dotnet user-secrets list --project .\Project.API`


### DateTime Provider

*__Application Layer__*
-  IDateTimeProvider :

```C#
 public interface IDatetimeProvider
    {
        DateTime UtcNow { get; }
    }
```

*__Infrastructure layer__*
-  DateTimeProvider

```C#
public class DateTimeProvider : IDatetimeProvider
    {
        public DateTime UtcNow => DateTime.UtcNow;
    }
```
--> Implement in Dependency injection

## Connexion Database MySql

To connect to you MySql DB, after having entities, create in Project.Infrastructure `DatabaseContext.cs`

Install on both Project.Infrastructure & Project.Api `Microsoft.EntityFrameworkCore`, `Microsoft.EntityFrameworkCore.Design`
And only on Infrastructure `Pomelo.EntityFrameworkCore.MySql`

Then on `DatabaseContext.cs`, you can configure the DbContext, and at the bottom, adding your Entities to share in your DB

```c#
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> opt) : base(opt)
        { }

        public DbSet<User> Users { get; set; } = null!;
    }
```

Get your `appsettings.json` and add this line to configure your database connexion and edit it with the right infos:

```json
  "ConnectionStrings":{
    "ProjectDatabase" : "Server=myServerAddress;Port=1234;Database=myDataBase;Uid=myUsername;Pwd=myPassword;"
  },

And after, you'll have to configure Database on `Program.cs` in API with this configuration :
```c#
    builder.Services.AddDbContextPool<DatabaseContext>(opt =>
    {
        var cs = builder.Configuration.GetConnectionString("ProjectDatabase");
        opt.UseMySql(cs, ServerVersion.AutoDetect(cs), b => b.MigrationsAssembly("Project.API"));
    });
```

Launch these commands in PM Console.

1.  To create the migration file : `dotnet ef migrations add init --project .\Project.API`
2.  To launch the creation of DB and make the first migration ` dotnet ef database update --project .\Project.API`

And for all you new updates of DB, make these two actions : First, create the file, second migrate.
If you want to abort one migration, before database update, you can type ` dotnet ef migrations remove --project .\Project.API`
It will delete the last migration file you generated.