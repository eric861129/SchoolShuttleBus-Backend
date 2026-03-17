using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SchoolShuttleBus.Infrastructure.Persistence;

namespace SchoolShuttleBus.Api.Tests;

public sealed class SchoolShuttleBusApiFactory : WebApplicationFactory<Program>
{
    private readonly SqliteConnection _connection = new("DataSource=:memory:");

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        _connection.Open();

        builder.UseEnvironment("Testing");
        builder.ConfigureAppConfiguration((_, configuration) =>
        {
            configuration.AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["ConnectionStrings:SchoolShuttleBus"] = "Data Source=unused",
                ["Jwt:Issuer"] = "SchoolShuttleBus.Tests",
                ["Jwt:Audience"] = "SchoolShuttleBus.Tests",
                ["Jwt:SigningKey"] = "integration-test-signing-key-1234567890"
            });
        });

        builder.ConfigureServices(services =>
        {
            services.RemoveAll<DbContextOptions<SchoolShuttleBusDbContext>>();
            services.RemoveAll<SchoolShuttleBusDbContext>();

            services.AddDbContext<SchoolShuttleBusDbContext>(options => options.UseSqlite(_connection));

            using var scope = services.BuildServiceProvider().CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<SchoolShuttleBusDbContext>();
            dbContext.Database.EnsureCreated();
            scope.ServiceProvider.GetRequiredService<SeedDataService>().SeedAsync().GetAwaiter().GetResult();
        });
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);
        if (disposing)
        {
            _connection.Dispose();
        }
    }
}
