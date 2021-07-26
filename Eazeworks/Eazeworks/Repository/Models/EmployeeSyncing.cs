using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Eazeworks.Repository.Models
{
    public partial class EmployeeSyncing
    {
        [Key]
        [Column("ID")]
        public int Id { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? SyncedOn { get; set; }
        [StringLength(50)]
        public string SyncedBy { get; set; }
    }
}
