using System;
using System.Linq;
using MasteringEFCore.Data;
using MasteringEFCore.Domain;
using Microsoft.EntityFrameworkCore;

namespace MasteringEFCore
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            TryCustomConverter();
            // ConvertValue();

            //GenerateSchema();
            // SeedData();
            // Setup();
        }

        static void TryCustomConverter()
        {
            using var db = new ApplicationContext();

            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            db.Converters.Add(
                new Converter
                {
                    Status = Status.Returned
                }
            );

            db.SaveChanges();

            var converterUnderAnalysis = db.Converters.AsNoTracking()
                                                      .FirstOrDefault(p => p.Status == Status.Analysis);

            var converterUnderReturned = db.Converters.AsNoTracking()
                                                      .FirstOrDefault(p => p.Status == Status.Returned);
        }

        static void ConvertValue() => GenerateScript();

        static void GenerateScript()
        {
            using var db = new ApplicationContext();

            var script = db.Database.GenerateCreateScript();
            
            Console.WriteLine(script);
        }

        static void GenerateSchema()
        {
            using var db = new ApplicationContext();

            var script = db.Database.GenerateCreateScript();
            
            Console.WriteLine(script);
        }

        static void SeedData()
        {
            using var db = new ApplicationContext();

            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            var script = db.Database.GenerateCreateScript();
            Console.WriteLine(script);
        }

        static void Setup()
        {
            using var db = new ApplicationContext();

            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            // Setup(db);

            // var description = "Department 01";
            // var departments = db.Departments.Where(d => d.Description == description).ToList();

            // Console.WriteLine("==============================");
            // Console.WriteLine("QueryDepartments");

            // foreach (var department in departments)
            // {
            //     Console.WriteLine($"Description: {department.Description} \t Deleted: {department.Deleted}");
            // }

            // Console.WriteLine("==============================");
        }

        // private static void Setup(ApplicationContext db)
        // {
        //     if (db.Database.EnsureCreated())
        //     {
        //         db.AddRange(
        //             new List<Department>()
        //             {
        //                 new Department
        //                 {
        //                     Description = "Department 01",
        //                     Active = true,
        //                     Deleted = true,
        //                     Employees = new List<Employee>()
        //                     {
        //                         new Employee
        //                         {
        //                             Name = "Employee 01",
        //                             PPS = "123456",
        //                             Document = "ABC123",
        //                             Deleted = false
        //                         },
        //                         new Employee
        //                         {
        //                             Name = "Employee 02",
        //                             PPS = "654321",
        //                             Document = "ABC456",
        //                             Deleted = false
        //                         }
        //                     }
        //                 },
        //                 new Department
        //                 {
        //                     Description = "Department 02",
        //                     Active = true,
        //                     Deleted = false,
        //                     Employees = new List<Employee>()
        //                     {
        //                         new Employee
        //                         {
        //                             Name = "Employee 03",
        //                             PPS = "753357",
        //                             Document = "ABC789",
        //                             Deleted = false
        //                         },
        //                         new Employee
        //                         {
        //                             Name = "Employee 04",
        //                             PPS = "951159",
        //                             Document = "DEF123",
        //                             Deleted = false
        //                         },
        //                         new Employee
        //                         {
        //                             Name = "Employee 05",
        //                             PPS = "159951",
        //                             Document = "DEF456",
        //                             Deleted = false
        //                         }
        //                     }
        //                 }
        //             }
        //         );

        //         db.SaveChanges();
        //     }
        // }
    }
}
