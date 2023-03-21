using Azure;
using Backend.Application.Common.Exceptions;
using Backend.Infrastructure.Persistence;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using NUnit.Framework;
using Respawn;

namespace Backend.Application.IntegrationTests;

[SetUpFixture]
public partial class Testing
{
    private static FakeCurrentUserService _currentUserService = null!;
    private static WebApplicationFactory<Program> _factory = null!;
    private static IConfiguration _configuration = null!;
    private static IServiceScopeFactory _scopeFactory = null!;
    private static Respawner _checkpoint = null!;

    [OneTimeSetUp]
    public async Task RunBeforeAnyTests()
    {
        _currentUserService = new FakeCurrentUserService();
        _factory = new CustomWebApplicationFactory(_currentUserService);
        _scopeFactory = _factory.Services.GetRequiredService<IServiceScopeFactory>();
        _configuration = _factory.Services.GetRequiredService<IConfiguration>();

        using var conn = new NpgsqlConnection(_configuration.GetConnectionString("Default")!);
        await conn.OpenAsync();
        _checkpoint = Respawner.CreateAsync(conn, new RespawnerOptions
        {
            DbAdapter = DbAdapter.Postgres,
            SchemasToInclude = new[] { "public" },
            TablesToIgnore = new Respawn.Graph.Table[] { "__EFMigrationsHistory" }
        }).GetAwaiter().GetResult();
    }

    public static async Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request)
    {
        using var scope = _scopeFactory.CreateScope();

        var mediator = scope.ServiceProvider.GetRequiredService<ISender>();

        return await mediator.Send(request);
    }

    public static async Task ShouldFailValidation<TResponse>(IRequest<TResponse> command, string error, string message) =>
        (await FluentActions.Invoking(() => SendAsync(command))
            .Should()
            .ThrowAsync<ValidationException>()
            .Where(ex => ex.Errors.ContainsKey(error))
        ).And.Errors[error].Should().Contain(message);


    public static string? CurrentUserId => _currentUserService.UserId;

    public static string? CurrentUserName => _currentUserService.UserName;


    public static string RunAsDefaultUser()
    {
        return RunAsUser("123-456-78910", "test");
    }

    public static string RunAsOtherUser()
    {
        return RunAsUser("123-456-78910-12331", "test-other");
    }

    public static string RunAsUser(string userId, string userName)
    {
        _currentUserService.UserId = userId;
        _currentUserService.UserName = userName;
        return _currentUserService.UserId;
    }

    public static async Task ResetState()
    {
        try
        {
            using var conn = new NpgsqlConnection(_configuration.GetConnectionString("Default")!);
            await conn.OpenAsync();

            await _checkpoint.ResetAsync(conn);
        }
        catch (Exception) { }
    }

    public static async Task<TEntity?> FindAsync<TEntity>(params object[] keyValues)
        where TEntity : class
    {
        using var scope = _scopeFactory.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        return await context.FindAsync<TEntity>(keyValues);
    }

    public static async Task AddAsync<TEntity>(TEntity entity)
        where TEntity : class
    {
        using var scope = _scopeFactory.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        context.Add(entity);

        await context.SaveChangesAsync();
    }

    public static async Task<int> CountAsync<TEntity>() where TEntity : class
    {
        using var scope = _scopeFactory.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        return await context.Set<TEntity>().CountAsync();
    }

    [OneTimeTearDown]
    public void RunAfterAnyTests()
    {
    }
}
