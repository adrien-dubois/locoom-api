using Locoom.Application.Common.Interfaces.Services;

namespace Locoom.Infrastructure.Services
{
    public class DateTimeProvider : IDatetimeProvider
    {
        public DateTime UtcNow => DateTime.UtcNow;
    }
}
