using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eazeworks.Models
{
    public class EmployeeVO
    {
        public int EmpDetId { get; set; }
        [BindProperty(Name = "EmployeeCode")]
        public string EmployeeCode { get; set; }

        public string EmployeeName { get; set; }

        public string ProjectName { get; set; }
        public string Designation { get; set; }
        public string Department { get; set; }

        public string SubDepartment { get; set; }

        public string WorkLocation { get; set; }

        public string Level { get; set; }

       
        public string Email { get; set; }

        public string ManagerEmail { get; set; }

        public string CountryCode { get; set; }

        public string Mobile { get; set; }

        public DateTime DateOfJoining { get; set; }

        public string Gender { get; set; }

        public string EmployeeType { get; set; }

        public string EmploymentStatus { get; set; }

        public string DeskNo { get; set; }

        public string PrevExp { get; set; }

        public string CurrentExperience { get; set; }

        public string TotalExperience { get; set; }

        public string Billing { get; set; }

        public string CoreSkills { get; set; }

        public string MinorSkills { get; set; }

        public string Certifications { get; set; }


        public string Customer { get; set; }
        
    }
}
