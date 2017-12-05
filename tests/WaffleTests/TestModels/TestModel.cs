using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WaffleTests.TestModels
{
    [Table("TestModel1")]
    public partial class TestModel1
    {
        /// <summary>
        /// Guid Property
        /// </summary>
        public Guid TmId { get; set; }

        /// <summary>
        /// Nullable Guid
        /// </summary>
        public Guid? TfkId { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public byte? PSI { get; set; }

        [StringLength(30)]
        public string ModifiedBy { get; set; }

        [Required]
        [StringLength(30)]
        public string CreatedBy { get; set; }

        public bool? Current { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}