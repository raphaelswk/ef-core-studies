namespace MasteringEFCore.Domain
{
    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }

        public Address Address { get; set; }
    }

    public class Address
    {
        public string Street { get; set; }
        public string Neighbour { get; set; }
        public string City { get; set; }
        public string State { get; set; }
    }
}
