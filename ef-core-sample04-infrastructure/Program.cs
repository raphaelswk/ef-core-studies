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

            ExecuteResilientStrategy();

            // TestCommandTimeout();
            
            // QueryDepartments();
        }

        static void ExecuteResilientStrategy()
        {
            /* THIS MUST BE USED WHEN SETTING THE RETRY STRATEGY IN ORDER TO AVOID 
               DUPLICATION IN CASE THERE IS A NETWORK FAILURE.
            */
            using var db = new ApplicationContext();

            var strategy = db.Database.CreateExecutionStrategy();
            strategy.Execute(() =>
            {
                using var transaction = db.Database.BeginTransaction();
                                
                db.Departments.Add(new Department { Description = "Transaction Department" });
                db.SaveChanges();

                transaction.Commit();
            });
        }

        static void TestCommandTimeout()
        {
            using var db = new ApplicationContext();

            db.Database.SetCommandTimeout(10);
            db.Database.ExecuteSqlRaw("WAITFOR DELAY '00:00:07'SELECT 1");

            // db.Database.ExecuteSqlRaw("SELECT 1");
        }

        static void QueryDepartments()
        {
            using var db = new ApplicationContext();
            // Setup(db);

            var description = "Department 01";
            var departments = db.Departments.Where(d => d.Description == description).ToList();

            Console.WriteLine("==============================");
            Console.WriteLine("QueryDepartments");

            foreach (var department in departments)
            {
                Console.WriteLine($"Description: {department.Description} \t Deleted: {department.Deleted}");
            }

            Console.WriteLine("==============================");
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
