using System.Collections.Generic;

namespace MasteringEFCore.Domain
{
    public class Movie
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<Actor> Actors { get; } = new List<Actor>();
    }
}