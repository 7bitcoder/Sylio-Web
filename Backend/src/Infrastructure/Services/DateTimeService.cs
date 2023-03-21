using Backend.Application.Common.Interfaces;

namespace Backend.Infrastructure.Services;

public class DateTimeService : IDateTime
{
    public DateTime Now => DateTime.UtcNow;
}
