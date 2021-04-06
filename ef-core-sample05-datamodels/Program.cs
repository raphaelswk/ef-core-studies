using System;
using System.Linq;
using System.Collections.Generic;
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

            TestPropertiesBag();
            // TestTPH();
            // TestSupportField();
            // TestManytoManyRelationship();
            // Test1toManyRelationship();
            // Test1to1Relationship();

            // WorkingWithOwnedTypes();
            // WorkingWithShadowProperties();
            // GenerateScript();

            // TryCustomConverter();
            // ConvertValue();

            //GenerateSchema();
            // SeedData();
            // Setup();
        }

        static void TestPropertiesBag()
        {
            using (var db = new ApplicationContext())
            {
                db.Database.EnsureDeleted();
                db.Database.EnsureCreated();

                var configuration = new Dictionary<string, object>
                {
                    ["Key"] = "DatabasePassword",
                    ["Value"] = Guid.NewGuid().ToString()
                };

                db.Configurations.Add(configuration);
                db.SaveChanges();

                var configurations = db.Configurations
                                       .AsNoTracking()
                                       .Where(p => p["Key"] as string == "DatabasePassword")
                                       .ToArray();
                
                foreach (var dic in configurations)
                {
                    Console.WriteLine($"Key: {dic["Key"]}, Value: {dic["Value"]}");
                }
            }
        }

        static void TestTPH()
        {
            using (var db = new ApplicationContext())
            {
                db.Database.EnsureDeleted();
                db.Database.EnsureCreated();

                var person = new Person { Name = "John Doe" };
                var teacher = new Teacher { Name = "John Tre", Technology = ".NET Core", Since = DateTime.Now };
                var student = new Student { Name = "John Qua", Age = 23, ContractDate = DateTime.Now.AddDays(-1) };

                db.AddRange(person, teacher, student);
                db.SaveChanges();

                var people = db.Persons.AsNoTracking().ToArray();
                var teachers = db.Teachers.AsNoTracking().ToArray();
                // var students = db.Students.AsNoTracking().ToArray();
                var students = db.Persons.OfType<Student>().AsNoTracking().ToArray();
                
                Console.WriteLine("=============== People ===============");                
                foreach (var p in people)
                {
                    Console.WriteLine($"Id: {p.Id}, Name: {p.Name}");
                }

                Console.WriteLine("============== Teachers ==============");
                foreach (var t in teachers)
                {
                    Console.WriteLine($"Id: {t.Id}, Name: {t.Name}, Technology: {t.Technology}, Since: {t.Since}");
                }

                Console.WriteLine("============== Students ==============");
                foreach (var s in students)
                {
                    Console.WriteLine($"Id: {s.Id}, Name: {s.Name}, Age: {s.Age}, ContractDate: {s.ContractDate}");
                }
            }
        }

        static void TestSupportField()
        {
            using (var db = new ApplicationContext())
            {
                db.Database.EnsureDeleted();
                db.Database.EnsureCreated();

                var document = new Document();
                document.SetPPS("123456879");

                db.Add(document);
                db.SaveChanges();

                foreach (var doc in db.Documents.AsNoTracking())
                {
                    Console.WriteLine($"PPS: {document.GetPPS()}");
                }
            }
        }

        static void TestManytoManyRelationship()
        {
            using (var db = new ApplicationContext())
            {
                db.Database.EnsureDeleted();
                db.Database.EnsureCreated();

                var actor1 = new Actor { Name = "John Uno" };
                var actor2 = new Actor { Name = "John Doe" };
                var actor3 = new Actor { Name = "John Tre" };

                var movie1 = new Movie { Name = "The return of the ones who had never gone" };
                var movie2 = new Movie { Name = "Back to the future" };
                var movie3 = new Movie { Name = "Dust on the sea" };

                actor1.Movies.Add(movie1);
                actor1.Movies.Add(movie2);

                actor2.Movies.Add(movie1);

                movie3.Actors.Add(actor1);
                movie3.Actors.Add(actor2);
                movie3.Actors.Add(actor3);

                db.AddRange(actor1, actor2, movie3);
                db.SaveChanges();

                foreach (var actor in db.Actors.Include(a => a.Movies))
                {
                    Console.WriteLine($"Actor: {actor.Name}");

                    foreach (var movie in actor.Movies)
                    {
                        Console.WriteLine($"\t Movie: {movie.Name}");
                    }
                }
            }
        }

        static void Test1toManyRelationship()
        {
            using (var db = new ApplicationContext())
            {
                db.Database.EnsureDeleted();
                db.Database.EnsureCreated();

                var state = new State
                {
                    Name = "California",
                    Governor = new Governor { Name = "Arnold Schwarzenegger" }
                };

                state.Cities.Add(new City { Name = "San Francisco" });

                db.States.Add(state);

                db.SaveChanges();                
            }

            using (var db = new ApplicationContext())
            {
                var states = db.States.ToList();

                states[0].Cities.Add(new City { Name = "Boca Raton" });
                db.SaveChanges();

                foreach (var stateItem in db.States.Include(s => s.Cities).AsNoTracking())
                {
                    Console.WriteLine($"State: {stateItem.Name} - Governor: {stateItem.Governor.Name}");

                    foreach (var city in stateItem.Cities)
                    {
                        Console.WriteLine($"City: {city.Name}");
                    }
                }
            }
        }

        static void Test1to1Relationship()
        {
            using var db = new ApplicationContext();
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            var state = new State
            {
                Name = "California",
                Governor = new Governor { Name = "Arnold Schwarzenegger" }
            };

            db.States.Add(state);

            db.SaveChanges();

            var states = db.States.AsNoTracking().ToList();

            states.ForEach(state =>
            {
                Console.WriteLine($"State: {state.Name} - Governor: {state.Governor.Name}");
            });
        }

        static void WorkingWithOwnedTypes()
        {
            using var db = new ApplicationContext();
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            var customer = new Customer
            {
                Name = "John Doe",
                Phone = "(83) 1234-987",
                Address = new Address { Neighbour = "City Centre", City = "Dublin" }
            };

            db.Customers.Add(customer);

            db.SaveChanges();

            var customers = db.Customers.AsNoTracking().ToList();

            var options = new System.Text.Json.JsonSerializerOptions { WriteIndented = true };

            customers.ForEach(c =>
            {
                var json = System.Text.Json.JsonSerializer.Serialize(c, options);

                Console.WriteLine(json);
            });
        }

        static void WorkingWithShadowProperties()
        {
            using var db = new ApplicationContext();
            /*
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            var department = new Department
            {
                Description = "Shadow Property Department"
            };

            db.Departments.Add(department);
            db.Entry(department).Property("LastUpdate").CurrentValue = DateTime.Now;
            db.SaveChanges();
            */

            var departments = db.Departments
                                .Where(d => EF.Property<DateTime>(d, "LastUpdate") < DateTime.Now)
                                .ToArray();
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
