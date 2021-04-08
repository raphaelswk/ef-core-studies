namespace MasteringEFCore.Domain
{
    public class Governor
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public string Party { get; set; }

        public int StateReference { get; set; }
        public State State { get; set; }
    }
}