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