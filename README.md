﻿17:05

# .NET 6

## Dependencies

*  API :
    - Mapster
    - Mapster.DependencyInjection
    - Microsoft.EntityFrameworkCore
    - Microsoft.EntityFrameworkCore.Design

*  Application
    - FluentValidation
    - FluentValidation.AspNetCore
    - MediatR
    - MediatR.Extensions.Microsoft.DependencyInjection
    - Microsoft.Extensions.DependencyInjection.Abstractions

*  Contracts
    
*  Domain
    - ErrorOr

*  Infrastructure
    - EntityFramework
    - Microsoft.AspNetCore.Authentication.JwtBearer
    - Microsoft.EntityFrameworkCore
    - Microsoft.EntityFrameworkCore.Design
    - Microsoft.Extensions.Configuration
    - Microsoft.Extensions.DependencyInjection.Abstractions
    - Microsoft.Extensions.Options.ConfigurationExtensions
    - Pomelo.EntityFrameworkCore.MySql
    - System.IdentityModel.Tokens.Jwt

## References

*  API :
    - Application
    - Contracts
    - Infrastructure

*  Application
    - Domain

*  Infrastructure
    - Application

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

```cs
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

```cs
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
```cs
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

## Handling Errors

First of all, install `Error-Or` package into Domain part.

Create Folders `/Common/Errors` in both Domain, Application & API parts. 
For example if you want to manage Invali credentials when authenticate, create `Errors.Authentication.cs` class into `Domain/Common/Errors`, with this logic :

```cs
    public static partial class Errors
    {
        public static class Authentication
        {

            public static Error InvalidCredentials => Error.Validation(
                code: "Auth.InvalidCredentials",
                description: "Utilisateur et/ou mot de passe incorrect(s)");
        }
    }
}
```

Create `ProjectProblemDetailsFactory.cs` in API Common/Error folder, and copy/paste ths class from OG aspnetcore project's [GitHub](https://github.com/dotnet/aspnetcore/blob/main/src/Mvc/Mvc.Core/src/Infrastructure/DefaultProblemDetailsFactory.cs)
with renaming class, and adding missing usings.

Add in **Program.cs** behind addControllers();
`builder.Services.AddSingleton<ProblemDetailsFactory, LocoomProblemDetailsFactory>();`
and in the build part, add on top : `app.UseExceptionHandler("/error");`

After this endpoint added, create a new folder into Api/Common : /Http and a new class : `HttpContextItemKeys.cs` with just that :
```cs
    public static class HttpContextItemKeys
    {
        public const string Errors = "errors";
    }
```


Then create 2 controllers in API :
*  ErrorsController
*  ApiController

#### ErrorsControler.cs
```cs
{
    public class ErrorsController : ControllerBase
    {
        [Route("/error")]
        public IActionResult Error()
        {
            Exception? exception = HttpContext.Features.Get<IExceptionHandlerFeature>()?.Error;


            return Problem();
        }
    }
}
```

#### ApiController.cs
```cs
{
    [ApiController]
    public class ApiController : ControllerBase
    {
        protected IActionResult Problem(List<Error> errors)
        {
            HttpContext.Items[HttpContextItemKeys.Errors] = errors;

            var firstError = errors[0];

            var statusCode = firstError.Type switch
            {
                ErrorType.Conflict => StatusCodes.Status409Conflict,
                ErrorType.Validation => StatusCodes.Status400BadRequest,
                ErrorType.NotFound => StatusCodes.Status404NotFound,
                _ => StatusCodes.Status500InternalServerError,
            };

            return Problem(statusCode: statusCode, title: firstError.Description);
        }
    }
}
```
In `AuthenticationService.cs`, front of methods with handling errors, add **ErrorOr** and put the name in <> like this :
`public ErrorOr<AuthenticationResult> Login`
And then in the error test, the moment to return, return it like this : `return Errors.Authentication.InvalidCredentials;`

In the `AuthenticationController.cs`, change the `:ControllerBase` with `:ApiController`, and return your method like this :

```cs
return authResult.Match(
                authResult => Ok(MapAuthResult(authResult)),
                errors => Problem(errors));
```

## CQRS

*  For the methods which are modifying, creating or touching any datas into database, in other things, those methods will be Commands methods.
*  For the others which are not touching datas, they are Queries methods

## Handler Pipeline

First create the pipeline in Application -> Common -> ValidationBehavior.cs
and a validator file in Application -> Authentication -> Command -> Register -> RegisterCommandValidator.cs

In the validator file, list all errors you can have like this :

```cs
    public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
    {
        public RegisterCommandValidator()
        {
            RuleFor(x => x.FirstName).NotEmpty();
            RuleFor(x => x.LastName).NotEmpty();
            RuleFor(x => x.Email).NotEmpty();
            RuleFor(x => x.Password).NotEmpty();
        }
    }
```

Start the Pipeline File, `ValidationBehavior.cs` with the `IPipelineBehavior` from **MediatR** , the command you need, with the ErrorOr result, the all in an Async Task Handle.

Inject the IValidator<RegisterCommand>.
Then put the validation result before the handler in the pipeline.
So if there are no validation errors, then we want to invoke our Handler, with `next` parameter
But if it's invalid, we have a list of validation failures (in the validation result variable), we want to convert them into a list of errors from the **ErroOr** librairy
and return them instead

```cs
public class ValidateRegisterCommandBehavior : IPipelineBehavior<RegisterCommand, ErrorOr<AuthenticationResult>>
    {
        private readonly IValidator<RegisterCommand> _validator;

        public ValidateRegisterCommandBehavior(IValidator<RegisterCommand> validator)
        {
            _validator = validator;
        }

        public async Task<ErrorOr<AuthenticationResult>> Handle(
            RegisterCommand request,
            CancellationToken cancellationToken,
            RequestHandlerDelegate<ErrorOr<AuthenticationResult>> next)
        {
            // Get the validator from request
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);

            // If no errors, then
            if(validationResult.IsValid)
            {
                // call the handler
                return await next();
            }

            // else, list it with ConvertAll which will make Select + ToList, to make a List of Error
            var errors = validationResult.Errors
                .ConvertAll(validationFailure => Error.Validation(
                                            validationFailure.PropertyName,
                                            validationFailure.ErrorMessage));

            return errors;
        }
    }
```
