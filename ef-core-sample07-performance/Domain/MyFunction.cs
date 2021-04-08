using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace MasteringEFCore.Domain
{
    public class MyFunction
    {
        public int Id { get; set; }

        [Column(TypeName = "NVARCHAR(100)")]        
        public string Description1 { get; set; }
        
        [Column(TypeName = "VARCHAR(100)")]
        public string Description2 { get; set; }

        public DateTime Date1 { get; set; }
        
        public string Date2 { get; set; }
    }
}