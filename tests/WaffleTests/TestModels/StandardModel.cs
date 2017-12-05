using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace WaffleTests.TestModels
{
    [Table("StandardModels", Schema = "dbo")]
    public class StandardModel
    {
        public int PK { get; set; }
        public int? FK { get; set; }
        public String Name { get; set; }

        public System.Text.StringBuilder StringBuilder { get; set; }
    }
}