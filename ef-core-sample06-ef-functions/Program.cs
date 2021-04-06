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
            TestCollateFunction();
            // TestPropertyFunction();
            // TestDataLengthFunction();
            // TestLikeFunction();
        }

        static void TestCollateFunction()
        {
            // Setup();
            using (var db = new ApplicationContext())
            {
                var result1 = db
                    .MyFunctions
                    .FirstOrDefault(p => EF.Functions.Collate(p.Description1, "SQL_Latin1_General_CP1_CS_AS") == "Ball B1");

                var result2 = db
                    .MyFunctions
                    .FirstOrDefault(p => EF.Functions.Collate(p.Description1, "SQL_Latin1_General_CP1_CI_AS") == "ball b1");

                Console.WriteLine($"Result1: {result1?.Description1}");
                Console.WriteLine($"Result2: {result2?.Description1}");
            }
        }

        static void TestPropertyFunction()
        {
            Setup();

            using (var db = new ApplicationContext())
            {
                var result = db
                    .MyFunctions
                    // .AsNoTracking()
                    .FirstOrDefault(p => EF.Property<string>(p, "MyShadowProperty") == "TEST");

                var shadowProperty = db.Entry(result)
                                       .Property<string>("MyShadowProperty")
                                       .CurrentValue;

                Console.WriteLine("Result: ");
                // SHADOW PROPERTY CAN JUST BE CALLED WHEN NOT USING AS NO TRACKING
                // BECAUSE EF CORE DOES NOT STORE THIS ANYWHERE
                Console.WriteLine(shadowProperty);
            }
        }

        static void TestDataLengthFunction()
        {
            Setup();

            using (var db = new ApplicationContext())
            {
                var result = db
                    .MyFunctions
                    .AsNoTracking()
                    .Select(p => new 
                    {
                        TotalBytesDate1Field = EF.Functions.DataLength(p.Date1),
                        TotalBytes1 = EF.Functions.DataLength(p.Description1),
                        TotalBytes2 = EF.Functions.DataLength(p.Description2),
                        Total1 = p.Description1.Length,
                        Total2 = p.Description2.Length
                    })
                    .FirstOrDefault();

                Console.WriteLine("Result: ");
                Console.WriteLine(result);
            }
        }

        static void TestLikeFunction()
        {
            Setup();

            using (var db = new ApplicationContext())
            {
                var result = db.MyFunctions.AsNoTracking()
                                           .Where(p => EF.Functions.Like(p.Description1, "C[ao]%"))
                                        //    .Where(p => EF.Functions.Like(p.Description1, "Can%"))
                                           .Select(p => p.Description1)
                                           .ToArray();

                Console.WriteLine("Result: ");

                foreach (var r in result)
                {
                    Console.WriteLine(r);
                }
            }
        }

        static void TestEFFunctions()
        {
            Setup();

            using (var db = new ApplicationContext())
            {
                var script = db.Database.GenerateCreateScript();

                Console.WriteLine(script);

                var result = db.MyFunctions.AsNoTracking().Select(p =>
                    new 
                    {
                        Days = EF.Functions.DateDiffDay(DateTime.Now, p.Date1),
                        Date = EF.Functions.DateFromParts(2021, 1, 2),
                        ValidDate = EF.Functions.IsDate(p.Date2)
                    }
                );

                foreach (var r in result)
                {
                    Console.WriteLine(r);
                }
            }
        }

        static void Setup()
        {
            using (var db = new ApplicationContext())
            {
                db.Database.EnsureDeleted();
                db.Database.EnsureCreated();

                db.MyFunctions.AddRange(
                    new MyFunction 
                    {
                        Date1 = DateTime.Now.AddDays(2),
                        Date2 = "2021-01-01",
                        Description1 = "Candy A1 ",
                        Description2 = "Candy A2 "
                    },
                    new MyFunction 
                    {
                        Date1 = DateTime.Now.AddDays(1),
                        Date2 = "XX21-01-01",
                        Description1 = "Ball B1",
                        Description2 = "Ball B2"
                    },
                    new MyFunction 
                    {
                        Date1 = DateTime.Now.AddDays(1),
                        Date2 = "XX21-01-01",
                        Description1 = "Coca-cola C1",
                        Description2 = "Pen C2"
                    }
                );

                db.SaveChanges();
            }
        }
    }
}
