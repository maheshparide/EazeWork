using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Eazeworks.Repository.Models
{
    public partial class EmployeeDetail
    {
        [Key]
        [Column("EmpDetID")]
        public int EmpDetId { get; set; }
        [Column("EmailID")]
        [StringLength(50)]
        public string EmailId { get; set; }
        [StringLength(50)]
        public string FirstName { get; set; }
        [StringLength(50)]
        public string LastName { get; set; }
        public string Address { get; set; }
        public string Address1 { get; set; }
        [StringLength(50)]
        public string City { get; set; }
        [StringLength(50)]
        public string Company { get; set; }
        [StringLength(50)]
        public string Country { get; set; }
        [Column("DOB", TypeName = "date")]
        public DateTime? Dob { get; set; }
        [Column("DOJ", TypeName = "date")]
        public DateTime? Doj { get; set; }
        [StringLength(50)]
        public string DataChange { get; set; }
        [StringLength(50)]
        public string Department { get; set; }
        [StringLength(50)]
        public string Designation { get; set; }
        [StringLength(50)]
        public string Division { get; set; }
        [StringLength(50)]
        public string EmpType { get; set; }
        [StringLength(50)]
        public string EmploymentStatus { get; set; }
        [StringLength(50)]
        public string FunctionalManager { get; set; }
        [StringLength(50)]
        public string FunctionalManagerCode { get; set; }
        [StringLength(50)]
        public string Gender { get; set; }
        [Column("LWD")]
        [StringLength(50)]
        public string Lwd { get; set; }
        [MaxLength(50)]
        public string Level { get; set; }
        [StringLength(50)]
        public string Manager { get; set; }
        [StringLength(50)]
        public string ManagerCode { get; set; }
        [StringLength(50)]
        public string MiddleName { get; set; }
        [StringLength(50)]
        public string MobileNo { get; set; }
        [StringLength(50)]
        public string Name { get; set; }
        [Column("NewTranID")]
        [StringLength(50)]
        public string NewTranId { get; set; }
        public string OfficeLocation { get; set; }
        public string OfficeLocationCity { get; set; }
        [Column(TypeName = "date")]
        public DateTime? OnDate { get; set; }
        [StringLength(50)]
        public string Pin { get; set; }
        [Column(TypeName = "date")]
        public DateTime? SepSubmissionDate { get; set; }
        [StringLength(50)]
        public string State { get; set; }
        [StringLength(50)]
        public string Status { get; set; }
        [StringLength(50)]
        public string WorkLocation { get; set; }
        [StringLength(50)]
        public string WorkLocationCity { get; set; }
        [StringLength(50)]
        public string WorkNature { get; set; }
        [StringLength(50)]
        public string EmpCode { get; set; }
        [Column(TypeName = "date")]
        public DateTime? EnteredDate { get; set; }
        [Column(TypeName = "date")]
        public DateTime? Changedate { get; set; }
        public string CustomAdditional { get; set; }
        public string CustomOfficial { get; set; }
        public string CustomPersonal { get; set; }
    }
}
