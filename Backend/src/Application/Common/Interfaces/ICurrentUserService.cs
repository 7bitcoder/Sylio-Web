using System.Security.Claims;

namespace Backend.Application.Common.Interfaces;

public interface ICurrentUserService
{
    string? UserId { get; }

    string? UserName { get; }
}
