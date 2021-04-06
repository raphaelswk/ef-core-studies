using System;
using System.Collections.Generic;
using System.Reflection;
using MasteringEFCore.Configurations;
using MasteringEFCore.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MasteringEFCore.Data
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Department> Departments { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<State> States { get; set; }

        public DbSet<Converter> Converters { get; set; }
        public DbSet<Customer> Customers { get; set; }
        
        public DbSet<Actor> Actors { get; set; }
        public DbSet<Movie> Movies { get; set; }

        public DbSet<Document> Documents { get; set; }
        
        public DbSet<Person> Persons { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Student> Students { get; set; }

        public DbSet<Dictionary<string, object>> Configurations => Set<Dictionary<string, object>>("Configurations");

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            const string strConnection = "Server=localhost,1433;Database=MasteringEFCoreDB-05;User Id=sa;Password=#MyPass123;Trusted_Connection=false;MultipleActiveResultSets=true";
            optionsBuilder
                .UseSqlServer(strConnection)
                .EnableSensitiveDataLogging()
                .LogTo(Console.WriteLine, LogLevel.Information);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // PROPERTIES BAG
            modelBuilder.SharedTypeEntity<Dictionary<string, object>>("Configurations", b =>
            {
                b.Property<int>("Id");

                b.Property<string>("Key")
                 .HasColumnType("VARCHAR(40)")
                 .IsRequired();

                b.Property<string>("Value")
                 .HasColumnType("VARCHAR(255)")
                 .IsRequired();
            });

            // modelBuilder.ApplyConfiguration(new CustomerConfiguration());
            // LINES BELOW IMPORT ALL THE CLASSES IMPLEMENTING THE INTERFACE IEntityTypeConfiguration
            // FROM THE CURRENT ASSEMBLY. SO THERE IS NO NEED TO IMPORT ONE BY ONE
            // modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationContext).Assembly);

            // // SHADOW PROPERTIES
            // modelBuilder.Entity<Department>().Property<DateTime>("LastUpdate");

            // // CONVERSION

            // // All existing converters
            // // Microsoft.EntityFrameworkCore.Storage.ValueConversion.
            // // ValueConverter is to be used for specific needs

            // var conversion = 
            //     new ValueConverter<MasteringEFCore.Domain.Version, string>(
            //         p => p.ToString(), 
            //         p => (MasteringEFCore.Domain.Version)Enum.Parse(typeof(MasteringEFCore.Domain.Version), p));

            // var conversion2 = new EnumToStringConverter<MasteringEFCore.Domain.Version>();

            // modelBuilder.Entity<Converter>()
            //             .Property(p => p.Version)
            //             .HasConversion(conversion2);
            //             // .HasConversion(conversion);
            //             // .HasConversion(p => p.ToString(), // Makes EF Core save it on DB as String
            //             //                                   // When EF Core reads it, convert to chosen enum
            //             //                p => (MasteringEFCore.Domain.Version)Enum.Parse(typeof(MasteringEFCore.Domain.Version), p));
            //             // .HasConversion<string>();

            // modelBuilder.Entity<Converter>()
            //             .Property(p => p.Status)
            //             .HasConversion(new CustomConverter());

            // // SCHEMAS
            // modelBuilder.HasDefaultSchema("registrations");
            // modelBuilder.Entity<State>().ToTable("States", "SecondSchema");

            // SEED DATA
            // modelBuilder.Entity<State>().HasData(new[]
            // {
            //     new State { Id = 1, Name = "Sao Paulo" },
            //     new State { Id = 2, Name = "Rio de Janeiro" }
            // });

            // // INDEXES
            // modelBuilder.Entity<Department>()
            //             // .HasIndex(p => p.Description)
            //             .HasIndex(p => new { p.Description, p.Active })
            //             .HasDatabaseName("idx_my_compound_index")
            //             .HasFilter("Description IS NOT NULL")
            //             .HasFillFactor(80)
            //             .IsUnique();

            // // COLLATIONS
            // /*
            //     CI - Case Insensitive
            //     CS - Case Sensitive
            //     AI - Accentuation Insensitive
            //     AS - Accentuation Sensitive
            // */

            // // GLOBAL SETTING
            // modelBuilder.UseCollation("SQL_Latin1_General_CP1_CI_AI");
            
            // // SPECIFIC SETTING
            // modelBuilder.Entity<Department>()
            //             .Property(p => p.Description)
            //             .UseCollation("SQL_Latin1_General_CP1_CS_AS");

            // // SEQUENCES
            // modelBuilder.HasSequence<int>("MySequence", "sequences")
            //             .StartsAt(1)
            //             .IncrementsBy(2)
            //             .HasMin(1)
            //             .HasMax(10)
            //             .IsCyclic();

            // modelBuilder.Entity<Department>()
            //             .Property(p => p.Id)
            //             .HasDefaultValueSql("NEXT VALUE FOR sequences.MySequence");
        }
    }
}
