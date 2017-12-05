using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace WaffleTests.TestModels
{
    [Table("EachTypeWeHave")]
    public class EachTypeWeHave
    {
        public int PK { get; set; }
        public int? FK { get; set; }
        public Guid theGuid { get; set; }
        public Guid? nullGuid { get; set; }
        public string Name { get; set; }
        public DateTime Dt { get; set; }
        public DateTime? NullableDt { get; set; }
        public double d { get; set; }
        public double? dNull { get; set; }
        public long longNumber { get; set; }
        public Int64 i64Number { get; set; }
        public float er { get; set; }
        public decimal dec { get; set; }
        public bool theBIT { get; set; }
        public Boolean theBOOLEAN { get; set; }
        public Byte chewer { get; set; }
        public byte bigChewer { get; set; }

        public byte[] binary { get; set; }
        public DateTimeOffset offset { get; set; }
    }
}