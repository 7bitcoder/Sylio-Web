using System.Security.Claims;

using Backend.Application.Common.Interfaces;

namespace WebApi.Services;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string? UserId => Get(ClaimTypes.NameIdentifier, "Server");

    public string? UserName => Get(ClaimTypes.Name, "Server");

    private string? Get(string claimType,  string server)
    {
        var context = _httpContextAccessor.HttpContext;
        if (context is not null)
        {
            return context.User?.FindFirstValue(claimType);
        }
        return server;
    }
}
