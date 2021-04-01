namespace MasteringEFCore.Domain
{
    public class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string PPS { get; set; }
        public int DepartmentId { get; set; }
        public virtual Department Department { get; set; }
    }
}