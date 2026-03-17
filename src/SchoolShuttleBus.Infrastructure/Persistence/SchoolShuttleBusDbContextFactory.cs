using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace SchoolShuttleBus.Infrastructure.Persistence;

public sealed class SchoolShuttleBusDbContextFactory : IDesignTimeDbContextFactory<SchoolShuttleBusDbContext>
{
    public SchoolShuttleBusDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<SchoolShuttleBusDbContext>();
        optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=SchoolShuttleBusDesignTime;Trusted_Connection=True;TrustServerCertificate=True;");
        return new SchoolShuttleBusDbContext(optionsBuilder.Options);
    }
}
