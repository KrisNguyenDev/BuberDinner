using BuberDinner.Application.Common.Interfaces.Services;

namespace BuberDinner.Infrastructure.Services;

public class DateTimeProvider : IDateTimeProvider
{
    public DateTime UtcNow { get => DateTime.UtcNow; }
}