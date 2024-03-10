using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Test.Server.Models;
using Test.Shared.Entities.DataBase;

namespace Test.Server.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(
            DbContextOptions options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
           // modelBuilder.ConfigurePersistedGrantContext(_operationalStoreOptions.Value);
            modelBuilder.Entity<Product>().Property("Id").HasDefaultValueSql("Newid()");
            modelBuilder.Entity<Product>().Property("Price").HasColumnType("decimal(12,2)");

        }
        public DbSet<ApplicationUser> AspNetUsers { get; set; }
        public DbSet<Test.Shared.Entities.DataBase.Product>? Product { get; set; }
    }
}
