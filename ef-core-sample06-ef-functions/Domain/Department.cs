using System.Collections.Generic;

namespace MasteringEFCore.Domain
{
    public class Department
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public bool Active { get; set; }
        public bool Deleted { get; set; }

        public List<Employee> Employees { get; set; }
    }
}
