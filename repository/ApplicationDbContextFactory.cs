using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using repository;

namespace PetAdoptionCenter.Repository
{
    public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseSqlServer("Server=database-pac-aws.c63m02oekbza.us-east-1.rds.amazonaws.com,1433;Database=PetAdoptionCenter;User Id=admin;Password=awsPac2025;TrustServerCertificate=True;");
            return new ApplicationDbContext(optionsBuilder.Options);
        }
    }
}
