using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace AutoGrader.Persistence.EF_Core;

public class AutoGraderDbContextFactory : IDesignTimeDbContextFactory<AutoGraderDbContext>
{
    public AutoGraderDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<AutoGraderDbContext>();
        optionsBuilder.UseSqlServer("Server=localhost,1433;Database=AutoGraderDb;User Id=sa;Password=password!1;TrustServerCertificate=True");

        return new AutoGraderDbContext(optionsBuilder.Options);
    }
}
