using System;
using System.ComponentModel.DataAnnotations.Schema;


namespace WaffleTests.TestModels
{
    [Table("TestNullAndNotNullInts")]
    public class TestNullAndNotNullInt
    {
        public int PK { get; set; }
        public int? FK { get; set; }
    }
}