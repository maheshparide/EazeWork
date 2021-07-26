using Eazeworks.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Eazeworks.Repository.Models
{
    public class Employee
    {
             
        public string EmailId { get; set; }
        
        public string FirstName { get; set; }
        
        public string LastName { get; set; }
        public string Address { get; set; }
        public string Address1 { get; set; }
       
        public string City { get; set; }
    
        public string Company { get; set; }
    
        public string Country { get; set; }
      
        public DateTime? Dob { get; set; }
       
        public DateTime? Doj { get; set; }
      
        public string DataChange { get; set; }
     
        public string Department { get; set; }
     
        public string Designation { get; set; }
    
        public string Division { get; set; }
      
        public string EmpType { get; set; }
      
        public string EmploymentStatus { get; set; }
   
        public string FunctionalManager { get; set; }
   
        public string FunctionalManagerCode { get; set; }
       
        public string Gender { get; set; }
     
        public string Lwd { get; set; }
   
        public byte[] Level { get; set; }
     
        public string Manager { get; set; }
       
        public string ManagerCode { get; set; }
     
        public string MiddleName { get; set; }
       
        public string MobileNo { get; set; }
      
        public string Name { get; set; }
       
     
        public string NewTranId { get; set; }
        public string OfficeLocation { get; set; }
        public string OfficeLocationCity { get; set; }
     
        public DateTime? OnDate { get; set; }
        public string Pin { get; set; }
   
        public DateTime? SepSubmissionDate { get; set; }
        public string State { get; set; }
        
        public string Status { get; set; }
      
        public string WorkLocation { get; set; }
    
        public string WorkLocationCity { get; set; }
      
        public string WorkNature { get; set; }
     
        public string EmpCode { get; set; }
       
        public DateTime? EnteredDate { get; set; }
    
        public DateTime? Changedate { get; set; }
        public CustomAdditional[] CustomAdditional { get; set; }
        public CustomOfficial[] CustomOfficial { get; set; }
        public CustomPersonals[] CustomPersonal { get; set; }
    }
}
