using System.Collections.Generic;

namespace MasteringEFCore.Domain
{
    public class Actor
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<Movie> Movies { get; } = new List<Movie>();
    }
}