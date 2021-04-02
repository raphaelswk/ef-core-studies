using System;
using System.Collections.Generic;
using System.Linq;
using MasteringEFCore.Data;
using MasteringEFCore.Domain;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace MasteringEFCore
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            QueryViaStoredProcedure();
            // CreateQueryStoredProcedure();
            // InsertingDataViaStoredProcedure();
            // CreateStoredProcedure();
            
            // SplitQuery();
            // QueryTagWith();
            
            // SqlRawQuery();
            // SelectedQuery();
            // GlobalFilter();
            // IgnoreGlobalFilter();
        }
        static void QueryViaStoredProcedure()
        {
            using var db = new ApplicationContext();

            var pDep = new SqlParameter("@pDep", "via");

            var departments = db.Departments
                                // .FromSqlRaw("EXECUTE GetDepartments @pDep", pDep)
                                // .FromSqlRaw("EXECUTE GetDepartments @p0", "Via")
                                .FromSqlInterpolated($"EXECUTE GetDepartments {pDep}")
                                .ToList();

            foreach (var department in departments)
            {
                Console.WriteLine(department.Description);
            }
        }

        static void CreateQueryStoredProcedure()
        {
            var createqQueryDepartmentSP = @"
                CREATE OR ALTER PROCEDURE GetDepartments
                    @Description VARCHAR(50)
                AS
                BEGIN
                    SELECT [Id]
                         , [Description]
                         , [Active]
                         , [Deleted]
                      FROM [Departments]
                     WHERE [Description] LIKE '%' + @Description + '%'
                END
            ";

            using var db = new ApplicationContext();
            db.Database.ExecuteSqlRaw(createqQueryDepartmentSP);
        }

        static void InsertingDataViaStoredProcedure()
        {
            using var db = new ApplicationContext();
            db.Database.ExecuteSqlRaw("EXECUTE CreateDepartment @p0, @p1", 
                                      "Department via SP", 
                                      true);
        }

        static void CreateStoredProcedure()
        {
            var createDepartment = @"
                CREATE OR ALTER PROCEDURE CreateDepartment
                    @Description VARCHAR(50),
                    @Active bit
                AS
                BEGIN
                    INSERT INTO
                        Departments(Description, Active, Deleted)
                    VALUES (@Description, @Active, 0)
                END
            ";

            using var db = new ApplicationContext();
            db.Database.ExecuteSqlRaw(createDepartment);
        }

        static void SplitQuery()
        {
            using var db = new ApplicationContext();
            Setup(db);
            
            var departments = db.Departments
                                .Include(d => d.Employees)
                                // .AsSplitQuery()
                                .AsSingleQuery()
                                .ToList();

            Console.WriteLine("==============================");
            Console.WriteLine("SplitQuery");

            foreach (var department in departments)
            {
                Console.WriteLine($"Description: {department.Description}");            
            }

            Console.WriteLine("==============================");
        }

        static void QueryTagWith()
        {
            using var db = new ApplicationContext();
            Setup(db);
            
            var departments = db.Departments                                
                                .TagWith(@"HERE IS MY COMMENT
                                
                                SECOND COMMENT
                                THIRD COMMENT
                                ")
                                .ToList();

            Console.WriteLine("==============================");
            Console.WriteLine("QueryTagWith");

            foreach (var department in departments)
            {
                Console.WriteLine($"Description: {department.Description}");            
            }

            Console.WriteLine("==============================");
        }

        static void SqlRawQuery()
        {
            using var db = new ApplicationContext();
            Setup(db);

            var id = 1;
            // var id = new SqlParameter
            // {
            //     Value = 0,
            //     SqlDbType = System.Data.SqlDbType.Int
            // };
            var departments = db.Departments
                                // .FromSqlRaw("SELECT * FROM Departments")
                                // .Where(d => !d.Deleted)

                                //.FromSqlRaw("SELECT * FROM Departments WHERE Id >= {0}", id)
                                
                                .FromSqlInterpolated($"SELECT * FROM Departments WHERE Id > {id}")
                                .ToList();

            Console.WriteLine("==============================");
            Console.WriteLine("SqlRawQuery");

            foreach (var department in departments)
            {
                Console.WriteLine($"Description: {department.Description}");            
            }

            Console.WriteLine("==============================");
        }

        static void SelectedQuery()
        {
            using var db = new ApplicationContext();
            Setup(db);

            var departments = db.Departments
                                .Where(d => d.Id > 0)
                                .Select(d => new
                                {
                                    d.Description,
                                    Employees = d.Employees.Select(e => e.Name)
                                })
                                .ToList();

            Console.WriteLine("==============================");
            Console.WriteLine("SelectedQuery");

            foreach (var department in departments)
            {
                Console.WriteLine($"Description: {department.Description}");
            
                foreach (var employee in department.Employees)
                {
                    Console.WriteLine($"\t Employee: {employee}");
                }
            }

            Console.WriteLine("==============================");
        }
        
        static void IgnoreGlobalFilter()
        {
            using var db = new ApplicationContext();
            Setup(db);

            var departments = db.Departments.IgnoreQueryFilters().Where(d => d.Id > 0).ToList();

            Console.WriteLine("==============================");
            Console.WriteLine("IgnoreGlobalFilter");

            foreach (var department in departments)
            {
                Console.WriteLine($"Description: {department.Description} \t Deleted: {department.Deleted}");
            }

            Console.WriteLine("==============================");
        }

        static void GlobalFilter()
        {
            using var db = new ApplicationContext();
            Setup(db);

            var departments = db.Departments.Where(d => d.Id > 0).ToList();

            Console.WriteLine("==============================");
            Console.WriteLine("GlobalFilter");

            foreach (var department in departments)
            {
                Console.WriteLine($"Description: {department.Description} \t Deleted: {department.Deleted}");
            }

            Console.WriteLine("==============================");
        }

        private static void Setup(ApplicationContext db)
        {
            if (db.Database.EnsureCreated())
            {
                db.AddRange(
                    new List<Department>()
                    {
                        new Department
                        {
                            Description = "Department 01",
                            Active = true,
                            Deleted = true,
                            Employees = new List<Employee>()
                            {
                                new Employee
                                {
                                    Name = "Employee 01",
                                    PPS = "123456",
                                    Document = "ABC123",
                                    Deleted = false
                                },
                                new Employee
                                {
                                    Name = "Employee 02",
                                    PPS = "654321",
                                    Document = "ABC456",
                                    Deleted = false
                                }
                            }
                        },
                        new Department
                        {
                            Description = "Department 02",
                            Active = true,
                            Deleted = false,
                            Employees = new List<Employee>()
                            {
                                new Employee
                                {
                                    Name = "Employee 03",
                                    PPS = "753357",
                                    Document = "ABC789",
                                    Deleted = false
                                },
                                new Employee
                                {
                                    Name = "Employee 04",
                                    PPS = "951159",
                                    Document = "DEF123",
                                    Deleted = false
                                },
                                new Employee
                                {
                                    Name = "Employee 05",
                                    PPS = "159951",
                                    Document = "DEF456",
                                    Deleted = false
                                }
                            }
                        }
                    }
                );

                

                db.SaveChanges();
            }
        }
    }
}
