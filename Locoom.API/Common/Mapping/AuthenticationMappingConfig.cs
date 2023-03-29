using Locoom.Application.Authentication.Commands.Register;
using Locoom.Application.Authentication.Common;
using Locoom.Application.Authentication.Queries.Login;
using Locoom.Contracts.Authentication;
using Mapster;

namespace Locoom.API.Common.Mapping
{
    public class AuthenticationMappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<RegisterRequest, RegisterCommand>();

            config.NewConfig<LoginRequest, LoginQuery>();

            config.NewConfig<AuthenticationResult, AuthenticationResponse>()
                .Map(dest => dest, src => src.User);
        }
    }
}
