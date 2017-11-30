using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace WaffleTests.TestModels
{
    [Table("ModelWithAGetter", Schema = "dbo")]
    public class ModelWithAGetter
    {
        public int PK { get; set; }
        public int? FK { get; set; }
        public String Name { get; }
    }
}