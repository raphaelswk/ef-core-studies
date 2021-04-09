using System;
using Microsoft.EntityFrameworkCore;
using src.Domain;
using src.Providers;

namespace src.Data
{
    public class ApplicationContext : DbContext
    {
        // MULTI-TENANT: TABLE FIELD STRATEGY
        // private readonly TenantData _tenant;

        public ApplicationContext(DbContextOptions<ApplicationContext> options
                                // MULTI-TENANT: TABLE FIELD STRATEGY
                                // , TenantData tenant
        )
            : base(options)
        {
            // MULTI-TENANT: TABLE FIELD STRATEGY
            // _tenant = tenant;
        }
        
        public DbSet<Person> People { get; set; }
        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Person>().HasData(
                new Person { Id = 1, Name = "Person 1", TenantId = "tenant-1" },
                new Person { Id = 2, Name = "Person 2", TenantId = "tenant-2" },
                new Person { Id = 3, Name = "Person 3", TenantId = "tenant-3" }
            );

            modelBuilder.Entity<Product>().HasData(
                new Product { Id = 1, Description = "Product 1", TenantId = "tenant-1" },
                new Product { Id = 2, Description = "Product 2", TenantId = "tenant-2" },
                new Product { Id = 3, Description = "Product 3", TenantId = "tenant-3" }
            );

            // MULTI-TENANT: TABLE FIELD STRATEGY
            // modelBuilder.Entity<Person>().HasQueryFilter(p => p.TenantId == _tenant.TenantId);
            // modelBuilder.Entity<Product>().HasQueryFilter(p => p.TenantId == _tenant.TenantId);
        }
    }
}
