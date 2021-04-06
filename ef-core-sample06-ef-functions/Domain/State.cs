using System.Collections.Generic;

namespace MasteringEFCore.Domain
{
    public class State
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public Governor Governor { get; set; }

        public ICollection<City> Cities { get; } = new List<City>();
    }
}