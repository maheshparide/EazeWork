using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eazeworks.Repository.Models
{
    // ViewEmployeeResultResponse response = JsonConvert.DeserializeObject<ViewEmployeeResultResponse>(myJsonResponse); 
    public class CustomAdditional
    {
        public string FieldName { get; set; }
        public string FieldValue { get; set; }
    }

    public class CustomOfficial
    {
        public string FieldName { get; set; }
        public string FieldValue { get; set; }
    }
    public class CustomPersonal
    {
        public string FieldName { get; set; }
        public string FieldValue { get; set; }
    }

    public class Data
    {
        public string Address { get; set; }
        public string Address1 { get; set; }
        public object Address2 { get; set; }
        public string City { get; set; }
        public string Company { get; set; }
        public string Country { get; set; }
        public List<CustomAdditional> CustomAdditional { get; set; }
        public List<CustomOfficial> CustomOfficial { get; set; }
        public List<CustomPersonal> CustomPersonal { get; set; }
        public string DOB { get; set; }
        public string DOJ { get; set; }
        public string DataChange { get; set; }
        public string Department { get; set; }
        public string Designation { get; set; }
        public string Division { get; set; }
        public string EmailID { get; set; }
        public string EmpCode { get; set; }
        public string EmpType { get; set; }
        public string EmploymentStatus { get; set; }
        public string FirstName { get; set; }
        public string FunctionalManager { get; set; }
        public string FunctionalManagerCode { get; set; }
        public string Gender { get; set; }
        public string LWD { get; set; }
        public string LastName { get; set; }
        public string Level { get; set; }
        public string Manager { get; set; }
        public string ManagerCode { get; set; }
        public string MiddleName { get; set; }
        public string MobileNo { get; set; }
        public string Name { get; set; }
        public string NewTranID { get; set; }
        public string OfficeLocation { get; set; }
        public string OfficeLocationCity { get; set; }
        public string OnDate { get; set; }
        public string Pin { get; set; }
        public string SepSubmissionDate { get; set; }
        public string State { get; set; }
        public string Status { get; set; }
        public string WorkLocation { get; set; }
        public string WorkLocationCity { get; set; }
        public string WorkNature { get; set; }
    }

    public class ViewEmployeeResult
    {
        public int ErrorCode { get; set; }
        public string ErrorMessage { get; set; }
        public string MessageType { get; set; }
        public Data Data { get; set; }
    }

    public class ViewEmployeeResultResponse
    {
        public ViewEmployeeResult ViewEmployeeResult { get; set; }
    }


}
