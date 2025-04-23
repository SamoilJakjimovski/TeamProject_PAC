using domain.Identity;
using domain.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PetAdoptionCenter.Domain.Models;
using PetAdoptionCenter.Domain.OtherModels;
using System.Collections.Generic;

namespace repository
{
    public class ApplicationDbContext : IdentityDbContext<PetAdoptionCenterUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
      
        public DbSet<Pet> Pet { get; set; } = default!;
        public DbSet<AdoptionApplication> AdoptionApplication { get; set; } = default!;
        public DbSet<Agency> Agencies { get; set; } = default!;
        public DbSet<TravelPackage> TravelPackages { get; set; } = default!;



        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(
                   "Server=tcp:pacsqlserver.database.windows.net,1433;Initial Catalog=PetAdoptionCenterAppDb;Persist Security Info=False;User ID=pacadmin;Password=Admin123-;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;",
                    sqlServerOptionsAction: sqlOptions =>
                    {
                        sqlOptions.MigrationsAssembly("PetAdoptionCenter.Web");

                        // Enabling retry on transient failures
                        sqlOptions.EnableRetryOnFailure(
                            maxRetryCount: 5, // Optional: Customize the number of retry attempts
                            maxRetryDelay: TimeSpan.FromSeconds(30), // Optional: Customize the delay between retries
                            errorNumbersToAdd: null // Optional: Specify additional SQL error numbers to retry on
                        );
                    });
            }

        }

    }


}
