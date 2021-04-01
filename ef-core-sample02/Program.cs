using System;
using System.Collections.Generic;
using System.Linq;
using MasteringEFCore.Data;
using MasteringEFCore.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace MasteringEFCore
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            GetLazyLoad();
            // GetExplicitlyLoad();
            // GetEagerLoad();

            // GetDBScript();
            // GetPendingMigrations();

            // ExecuteSQL();

            // // warmup
            // new ApplicationContext().Departments.AsNoTracking().Any();
            // _count = 0;
            // ManagingConnectionState(false);
            // _count = 0;
            // ManagingConnectionState(true);

            // DBHealthCheck();
            // EnsureCreatedGapSample();
            // EnsureCreatedAndDeleted();
        }

        static void GetLazyLoad()
        {
            using var db = new ApplicationContext();
            // PopulateDB(db);
            
            // db.ChangeTracker.LazyLoadingEnabled = false;

            var departments = db.Departments.ToList();

            foreach (var department in departments)
            {
                Console.WriteLine("-------------------------");
                Console.WriteLine($"Department: {department.Description}");

                if (department.Employees?.Any() ?? false)
                {
                    foreach (var employee in department.Employees)
                    {
                        Console.WriteLine($"\tEmployee: {employee.Name}");
                    }
                }
                else
                {
                    Console.WriteLine("No employee loaded");                    
                }
            }
        }

        static void GetExplicitlyLoad()
        {
            using var db = new ApplicationContext();
            // PopulateDB(db);
            
            var departments = db.Departments.ToList();

            foreach (var department in departments)
            {
                if (department.Id == 2)
                {
                    // db.Entry(department).Collection(d => d.Employees).Load();
                    db.Entry(department).Collection(d => d.Employees)
                                        .Query()
                                        .Where(d => d.Id > 4)
                                        .ToList();
                }

                Console.WriteLine("-------------------------");
                Console.WriteLine($"Department: {department.Description}");

                if (department.Employees?.Any() ?? false)
                {
                    foreach (var employee in department.Employees)
                    {
                        Console.WriteLine($"\tEmployee: {employee.Name}");
                    }
                }
                else
                {
                    Console.WriteLine("No employee loaded");                    
                }
            }
        }

        static void GetEagerLoad()
        {
            using var db = new ApplicationContext();
            // PopulateDB(db);
            
            var departments = db.Departments.Include(d => d.Employees).ToList();

            foreach (var department in departments)
            {
                Console.WriteLine("-------------------------");
                Console.WriteLine($"Department: {department.Description}");
            }
        }

        private static void PopulateDB(ApplicationContext db)
        {
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            var departments = new List<Department>()
            {
                new Department
                {
                    Description = "Department 01",
                    Active = true,
                    Employees = new List<Employee>()
                    {
                        new Employee
                        {
                            Name = "Employee 01",
                            PPS = "123456"
                        },
                        new Employee
                        {
                            Name = "Employee 02",
                            PPS = "654321"
                        }
                    }
                },
                new Department
                {
                    Description = "Department 02",
                    Active = true,
                    Employees = new List<Employee>()
                    {
                        new Employee
                        {
                            Name = "Employee 03",
                            PPS = "753357"
                        },
                        new Employee
                        {
                            Name = "Employee 04",
                            PPS = "951159"
                        },
                        new Employee
                        {
                            Name = "Employee 05",
                            PPS = "159951"
                        }
                    }
                }
            };

            db.AddRange(departments);
            db.SaveChanges();
        }

        static void GetDBScript()
        {
            using var db = new ApplicationContext();
            var script = db.Database.GenerateCreateScript();
            Console.WriteLine(script);
        }

        static void GetPendingMigrations()
        {
            using var db = new ApplicationContext();
            
            // GET ALL MIGRATIONS
            var migrations = db.Database.GetMigrations();
            
            // GET ONLY PENDING
            var pendingMigrations = db.Database.GetPendingMigrations();

            // GET ONLY APPLIED
            var appliedMigrations = db.Database.GetAppliedMigrations();

            Console.WriteLine($"Total: {pendingMigrations.Count()}");

            foreach (var migration in migrations)
            {
                Console.WriteLine($"Migration: {migration}");
            }

            // Apply Migrations
            // db.Database.Migrate();
        }

        static void ExecuteSQL()
        {
            using var db = new ApplicationContext();

            // // First Option
            // using (var conn = db.Database.GetDbConnection())
            // using (var cmd = conn.CreateCommand())
            // {
            //     conn.Open();
            //     cmd.CommandText = "SELECT 1";
            //     cmd.ExecuteNonQuery();
            // }

            // db.Departments.AddRange(
            //     new Domain.Department
            //     {
            //         Description = "Department 01"
            //     },
            //     new Domain.Department
            //     {
            //         Description = "Department 02"
            //     }
            // );
            // db.SaveChanges();

            // // Second Option
            var newDescription = "Test 02";
            // db.Database.ExecuteSqlRaw("UPDATE DEPARTMENTS SET DESCRIPTION = {0} WHERE ID = 1", newDescription);

            // // Third Option
            // db.Database.ExecuteSqlInterpolated($"UPDATE DEPARTMENTS SET DESCRIPTION = {newDescription} WHERE ID = 1");

            // SQL Injection Attack
            newDescription = "Test ' or 1='1";
            db.Database.ExecuteSqlRaw($"UPDATE DEPARTMENTS SET DESCRIPTION = 'SQL Injection Attack' WHERE DESCRIPTION = '{newDescription}'");
            foreach (var department in db.Departments.AsNoTracking())
            {
                Console.WriteLine($"Id: {department.Id}, Desc: {department.Description}");
            }
        }

        static int _count;
        static void ManagingConnectionState(bool manageConnectionState)
        {
            using var db = new ApplicationContext();
            var time = System.Diagnostics.Stopwatch.StartNew();

            var connection = db.Database.GetDbConnection();
            connection.StateChange += (_, __) => ++_count;

            if (manageConnectionState)
            {
                connection.Open();
            }

            for (int i = 0; i < 300; i++)
            {
                db.Departments.AsNoTracking().Any();
            }

            time.Stop();
            
            var message = $"Time: {time.Elapsed.ToString()}, {manageConnectionState}, Counter: {_count}";

            Console.WriteLine(message);
        }

        static void DBHealthCheck()
        {
            using var db = new ApplicationContext();
            if (db.Database.CanConnect())
            {
                Console.WriteLine("Healthy");
            } 
            else
            {
                Console.WriteLine("Unhealthy");
            }
        }

        static void EnsureCreatedGapSample()
        {
            using var db1 = new ApplicationContext();
            using var db2 = new ApplicationContextCity();

            db1.Database.EnsureCreated();
            db2.Database.EnsureCreated();

            var databaseCreator = db2.GetService<IRelationalDatabaseCreator>();
            databaseCreator.CreateTables();
        }

        static void EnsureCreatedAndDeleted()
        {
            using var db = new ApplicationContext();
            // db.Database.EnsureCreated();
            db.Database.EnsureDeleted();
        }
    }
}
