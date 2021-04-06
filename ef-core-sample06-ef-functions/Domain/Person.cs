using System;

namespace MasteringEFCore.Domain
{
    public class Person
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class Teacher : Person
    {
        public DateTime Since { get; set; }
        public string Technology { get; set; }
    }

    public class Student : Person
    {
        public int Age { get; set; }
        public DateTime ContractDate { get; set; }
    }
}