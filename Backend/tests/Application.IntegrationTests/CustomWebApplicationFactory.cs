using Backend.Application.Common.Interfaces;
using Backend.Infrastructure.Persistence;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace Backend.Application.IntegrationTests;

using static Testing;

internal class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    private readonly ICurrentUserService _currentUserService;

    public CustomWebApplicationFactory(ICurrentUserService currentUserService) : base()
    {
        _currentUserService = currentUserService;
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureAppConfiguration(configurationBuilder =>
        {
            var integrationConfig = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables()
                .Build();

            configurationBuilder.AddConfiguration(integrationConfig);
        });

        builder.ConfigureServices((builder, services) =>
        {
            services
                .Remove<ICurrentUserService>()
                .AddTransient(provider => _currentUserService);

            services
                .Remove<DbContextOptions<ApplicationDbContext>>()
                .AddDbContext<ApplicationDbContext>((sp, options) =>
                    options.UseNpgsql(builder.Configuration.GetConnectionString("Default"),
                        builder => builder.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));
        });
    }
}
