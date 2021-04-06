namespace MasteringEFCore.Domain
{
    public class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string PPS { get; set; }
        public string Document { get; set; }
        public bool Deleted { get; set; }
        
        public int DepartmentId { get; set; }
        public Department Department { get; set; }
    }
}