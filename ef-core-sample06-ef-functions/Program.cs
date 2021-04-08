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
            GetDepartments();
        }

        static void GetDepartments()
        {
            using var db = new ApplicationContext();
            // db.Database.EnsureDeleted();
            // db.Database.EnsureCreated();

            // CHANGE TRACKING OPTION FOR THE CURRENT INSTANCE
            // db.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

            db.Departments
                // .AsTracking() // CHANGE TRACKING OPTION FOR SPECIFIC QUERY
                .Include(d => d.Employees)
                .ToList();

            var memoryUsage = (System.Diagnostics.Process.GetCurrentProcess().WorkingSet64 / 1024 / 1024) + "MB";
            Console.WriteLine(memoryUsage);

        }

        // static void Setup()
        // {
        //     using (var db = new ApplicationContext())
        //     {
        //         db.Database.EnsureDeleted();
        //         db.Database.EnsureCreated();

        //         db.MyFunctions.AddRange(
        //             new MyFunction 
        //             {
        //                 Date1 = DateTime.Now.AddDays(2),
        //                 Date2 = "2021-01-01",
        //                 Description1 = "Candy A1 ",
        //                 Description2 = "Candy A2 "
        //             },
        //             new MyFunction 
        //             {
        //                 Date1 = DateTime.Now.AddDays(1),
        //                 Date2 = "XX21-01-01",
        //                 Description1 = "Ball B1",
        //                 Description2 = "Ball B2"
        //             },
        //             new MyFunction 
        //             {
        //                 Date1 = DateTime.Now.AddDays(1),
        //                 Date2 = "XX21-01-01",
        //                 Description1 = "Coca-cola C1",
        //                 Description2 = "Pen C2"
        //             }
        //         );

        //         db.SaveChanges();
        //     }
        // }
    }
}
