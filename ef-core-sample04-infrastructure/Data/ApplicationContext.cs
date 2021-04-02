using System;
using MasteringEFCore.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MasteringEFCore.Data
{
    public class ApplicationContext : DbContext
    {
        // private readonly StreamWriter _writer = new StreamWriter("ef_core_log.txt", append: true);

        public DbSet<Department> Departments { get; set; }
        public DbSet<Employee> Employees { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            const string strConnection = "Server=localhost,1433;Database=MasteringEFCoreDB-04;User Id=sa;Password=#MyPass123;Trusted_Connection=false;MultipleActiveResultSets=true";
            optionsBuilder
                .UseSqlServer(strConnection, 
                              o => o.MaxBatchSize(100) // The Default value is 42 
                                    .CommandTimeout(5) // The Default value is 30 secs
                                    .EnableRetryOnFailure(4, TimeSpan.FromSeconds(10), null)) // Every DB has its own default
                // .EnableDetailedErrors() // THIS AFFECTS PERFORMANCE, MUST BE USED ONLY IN DEVELOPMENT
                .EnableSensitiveDataLogging()
                .LogTo(Console.WriteLine, LogLevel.Information);

                // .LogTo(_writer.WriteLine, LogLevel.Information);

                // .LogTo(Console.WriteLine, new[] 
                //         { 
                //             CoreEventId.ContextInitialized, 
                //             RelationalEventId.CommandExecuted 
                //         },
                //         LogLevel.Information,
                //         DbContextLoggerOptions.LocalTime | DbContextLoggerOptions.SingleLine);

                // .LogTo(Console.WriteLine, new[] 
                // { 
                //     CoreEventId.ContextInitialized, 
                //     RelationalEventId.CommandExecuted 
                // });

                // .LogTo(Console.WriteLine, new[] { RelationalEventId.CommandExecuted });                
        }

        // public override void Dispose()
        // {
        //     base.Dispose();
        //     _writer.Dispose();
        // }
    }
}