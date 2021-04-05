using System.Net;

namespace MasteringEFCore.Domain
{
    public class Converter
    {
        public int Id { get; set; }
        public bool Active { get; set; }
        public bool Deleted { get; set; }
        public Version Version { get; set; }
        public IPAddress IPAddress { get; set; }
        public Status Status { get; set; }
    }

    public enum Version
    {
        EFCore1,
        EFCore2,
        EFCore3,
        EFCore5
    }

    public enum Status
    {
        Analysis,
        Sent,
        Returned
    }
}
