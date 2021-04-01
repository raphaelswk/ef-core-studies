using Microsoft.EntityFrameworkCore;
using EfCore.ConsoleApp.Domain;
using Microsoft.Extensions.Logging;
using System.Linq;
using System;

namespace EfCore.ConsoleApp.Data
{
    public class ApplicationContext : DbContext
    {
        private  static readonly ILoggerFactory _logger = LoggerFactory.Create(p => p.AddConsole());

        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<Produto> Produtos { get; set; }
        public DbSet<Cliente> Clientes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseLoggerFactory(_logger)
                .EnableSensitiveDataLogging()
                .UseSqlServer("Server=localhost,1433;Database=CursoEfCore01;User Id=sa;Password=#MyPass123;Trusted_Connection=false;MultipleActiveResultSets=true",
                    p => p.EnableRetryOnFailure(maxRetryCount: 2, 
                                                maxRetryDelay: TimeSpan.FromSeconds(5), 
                                                errorNumbersToAdd: null).MigrationsHistoryTable("my_custom_history"));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // modelBuilder.ApplyConfiguration(new ClienteConfiguration());
            // modelBuilder.ApplyConfiguration(new ProdutoConfiguration());
            // modelBuilder.ApplyConfiguration(new PedidoConfiguration());
            // modelBuilder.ApplyConfiguration(new PedidoItemConfiguration());

            // GET ALL CLASSES WHICH IMPLEMENTS IEntityTypeConfiguration IN THE ASSEMBBLY
            // AND ADD THEM TO THE CONFIGURATIONS
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationContext).Assembly);
            MapForgottenProperties(modelBuilder);
        }

        private void MapForgottenProperties(ModelBuilder modelBuilder)
        {
            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
                var properties = entity.GetProperties()
                                       .Where(p => p.ClrType == typeof(string));
                
                foreach (var property in properties)
                {
                    if (string.IsNullOrEmpty(property.GetColumnType()) &&
                        !property.GetMaxLength().HasValue)
                    {
                        property.SetMaxLength(100);
                        property.SetColumnType("VARCHAR(100)");
                    }
                }
            }
        }
    }
}