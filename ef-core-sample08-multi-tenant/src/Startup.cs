using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using src.Data;
using src.Domain;
using src.Extensions;
using src.Middlewares;
using src.Providers;

namespace EfCore.Mulitenant
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<TenantData>();

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "EfCore.Mulitenant", Version = "v1" });
            });

            // MULTI-TENANT: TABLE FIELD STRATEGY
            // services.AddDbContext<ApplicationContext>(p => p
            //             .UseSqlServer("Server=localhost,1433;Database=MasteringEFCoreDB-08;User Id=sa;Password=#MyPass123;Trusted_Connection=false;MultipleActiveResultSets=true")
            //             .LogTo(Console.WriteLine)
            //             .EnableDetailedErrors(true));

            // MULTI-TENANT: DATABASE STRATEGY
            services.AddHttpContextAccessor();

            services.AddScoped<ApplicationContext>(provider => 
            {
                var optionsBuilder = new DbContextOptionsBuilder<ApplicationContext>();

                var httpContext = provider.GetService<IHttpContextAccessor>()?.HttpContext;
                var tenantId = httpContext?.GetTenantId();

                // var connectionString = Configuration.GetConnectionString(tenantId);
                var connectionString = Configuration.GetConnectionString("custom").Replace("_DATABASE_", tenantId);

                optionsBuilder
                        .UseSqlServer(connectionString)
                        .LogTo(Console.WriteLine)
                        .EnableDetailedErrors(true);

                return new ApplicationContext(optionsBuilder.Options);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "EfCore.Mulitenant v1"));
            }

            // DatabaseInitialize(app);

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            // MULTI-TENANT: TABLE FIELD STRATEGY
            // app.UseMiddleware<TenantMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        // private void DatabaseInitialize(IApplicationBuilder app)
        // {
        //     using var db = app.ApplicationServices
        //                       .CreateScope()
        //                       .ServiceProvider
        //                       .GetRequiredService<ApplicationContext>();

        //     db.Database.EnsureDeleted();
        //     db.Database.EnsureCreated();

        //     for (int i = 1; i <= 5; i++)
        //     {
        //         db.People.Add(new Person { Name = $"Person {i}" });
        //         db.Products.Add(new Product { Description = $"Product {i}" });
        //     }

        //     db.SaveChanges();
        // }
    }
}
