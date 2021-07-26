using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Eazeworks.Repository.Models;
using Eazeworks.Models;
using Newtonsoft.Json;
using System.Text;
using Eazeworks.Repository;
using Newtonsoft.Json.Linq;
using System.IO;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using Data = Eazeworks.Models.Data;
using Microsoft.AspNetCore.Hosting;

namespace Eazeworks.Controllers
{
    public class EmployeeController : Controller
    {


        private EmployeeDbContext _dbContext;
        private IHostingEnvironment _hostingEnvironment;
        public List<EmployeeVO> employeeDetails;
        public List<string> userSelectedProps;


        public EmployeeController(EmployeeDbContext dbContext, IHostingEnvironment hostingEnvironment) {
            _dbContext = dbContext;
            _hostingEnvironment = hostingEnvironment;
        }
        // GET: EmployeeController
        public ActionResult Index()
        {
            List<EmployeeSyncing> lisEmpSync = _dbContext.EmployeeSyncing.ToList();
            DateTime dfrom = DateTime.ParseExact(lisEmpSync[lisEmpSync.Count - 1].SyncedOn.ToString(), "dd-MM-yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None);
            ViewBag.lastSyncedDate = dfrom.ToShortDateString().Replace("-", "/");
            return View();
        }

        // GET: EmployeeController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }
        public ActionResult EmployeesList(IFormCollection collection)
        {
            userSelectedProps = new List<string>();
            foreach (var key in collection.Keys)
            {
                userSelectedProps.Add(key) ;
            }
            ViewBag.UserSelectedProperties = userSelectedProps;
            if (userSelectedProps.Count > 0)
            {
                HttpContext.Session.SetObjectAsJson("userSelectedProps", userSelectedProps);
            }
            //List<string> reqiredProperties = new List<string>();
            //reqiredProperties.Add("EmployeeCode");
            //reqiredProperties.Add("EmployeeName");
            //reqiredProperties.Add("ProjectName");
            //reqiredProperties.Add("Designation");
            //reqiredProperties.Add("Department");
            //reqiredProperties.Add("SubDepartment");
            //reqiredProperties.Add("WorkLocation");
            //reqiredProperties.Add("Level");
            //reqiredProperties.Add("Email");
            //reqiredProperties.Add("ManagerEmail");
            //reqiredProperties.Add("CountryCode");
            //reqiredProperties.Add("Mobile");
            //reqiredProperties.Add("DateOfJoining");
            //reqiredProperties.Add("Gender");
            //reqiredProperties.Add("EmployeeType");
            //reqiredProperties.Add("EmploymentStatus");
            //reqiredProperties.Add("DeskNo");
            //reqiredProperties.Add("PrevExp");
            //reqiredProperties.Add("CurrentExperience");
            //reqiredProperties.Add("TotalExperience");
            //reqiredProperties.Add("Billing");
            //reqiredProperties.Add("CoreSkills");
            //reqiredProperties.Add("MinorSkills");
            //reqiredProperties.Add("Certifications");
            //reqiredProperties.Add("Customer");
            //ViewBag.CheckBoxProps = reqiredProperties;

            List <EmployeeDetail> empList = _dbContext.EmployeeDetails.ToList();
            List<EmployeeVO> gridList = new List<EmployeeVO>();
            foreach(EmployeeDetail e in empList)
            {
                EmployeeVO employeeVO = new EmployeeVO();
                employeeVO.EmpDetId = e.EmpDetId;
                employeeVO.EmployeeCode = e.EmpCode;
                employeeVO.EmployeeName = e.FirstName + " " + e.MiddleName + " " + e.LastName;
                List<CustomAdditional> customAdditional = JsonConvert.DeserializeObject<List<CustomAdditional>>(e.CustomAdditional);
                List<CustomOfficial> customOfficial = JsonConvert.DeserializeObject<List<CustomOfficial>>(e.CustomOfficial);
                employeeVO.ProjectName = customOfficial[2].FieldValue;
                CustomAdditional coreskill = customAdditional.Find(s => s.FieldName == "Core Skills (>=7/10 rating)");
                if (coreskill != null)
                {
                   employeeVO.CoreSkills= coreskill.FieldValue;
                }
                CustomAdditional minorskill = customAdditional.Find(s => s.FieldName == "Minor Skills (<=6/10 rating)");
                if (minorskill != null)
                {
                     employeeVO.MinorSkills= minorskill.FieldValue;
                }
                CustomAdditional cerfication = customAdditional.Find(s => s.FieldName == "Certifications(include certification org)");
                if (cerfication != null)
                {
                   employeeVO.Certifications= cerfication.FieldValue;
                }
                employeeVO.Customer=  customOfficial[1].FieldValue;
                employeeVO.Designation = e.Designation;
                employeeVO.DeskNo = "";
                if (e.Doj != null)
                {
                    employeeVO.DateOfJoining = DateTime.ParseExact(e.Doj.ToString(), "dd-MM-yyyy HH:mm:ss", null);
                }
                employeeVO.Department = e.Department;
                employeeVO.WorkLocation = e.WorkLocation;
                employeeVO.Level = e.Level;
                employeeVO.Email = e.EmailId;
                EmployeeDetail ed = empList.Find(s => s.EmpCode == e.ManagerCode);
                if (ed != null)
                {
                    employeeVO.ManagerEmail = ed.EmailId;
                }
                employeeVO.Mobile = e.MobileNo;
                employeeVO.Gender = e.Gender;
                employeeVO.EmployeeType = e.EmpType;
                employeeVO.EmploymentStatus = e.EmploymentStatus;
                employeeVO.DeskNo = "";
                DateTime d1 = DateTime.Now;
                DateTime d2 = DateTime.ParseExact(e.Doj.ToString(), "dd-MM-yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None);
                employeeVO.CurrentExperience= new DateDifference(d1, d2).ToString();
                employeeVO.PrevExp = customAdditional[0].FieldValue;
                employeeVO.TotalExperience = "";
                employeeVO.Billing = customOfficial[0].FieldValue;
                if(e.EmpCode != null)
                {
                    if (e.EmpCode.Contains("MLI")) {
                        employeeVO.CountryCode = "91";
                    } else {
                        employeeVO.CountryCode = "01";
                    }  
                }

                gridList.Add(employeeVO);
            }
            if (gridList.Count > 0)
            {
                employeeDetails = gridList;
            }
            return View(gridList);
        }

        public async Task<ActionResult> saveMLICAsync()
        {
            if (ModelState.IsValid)
            {
                using (var client = new HttpClient())
                {
                    for (int a = 0; a < 10; a++)
                    {
                        for (int b = 0; b < 10; b++)
                        {
                           
                                var emp = new EmployeeData { APIKey = "32a75a44-c927-42fb-b8ec-8d0898880d90", CorpURL = "https://motivitylabs.com", EmpCode = "MLIC" + a + "" + b };
                                var Response = await client.PostAsJsonAsync("https://motivity.eazework.com/EazeAPI/Employee.svc/ViewEmployee", emp);
                                string empobj = await Response.Content.ReadAsStringAsync();
                                ViewEmployeeResultResponse response = JsonConvert.DeserializeObject<ViewEmployeeResultResponse>(empobj);
                                if (response.ViewEmployeeResult.ErrorCode == 0)
                                {
                                    if (response != null)
                                    {
                                        EmployeeDetail empl = new EmployeeDetail();
                                        empl.EmailId = response.ViewEmployeeResult.Data.EmailID;
                                        empl.FirstName = response.ViewEmployeeResult.Data.FirstName;
                                        empl.LastName = response.ViewEmployeeResult.Data.LastName;
                                        empl.Address = response.ViewEmployeeResult.Data.Address;
                                        empl.Address1 = response.ViewEmployeeResult.Data.Address1;
                                        empl.City = response.ViewEmployeeResult.Data.City;
                                        empl.Company = response.ViewEmployeeResult.Data.Company;
                                        empl.Country = response.ViewEmployeeResult.Data.Country;
                                        if (response.ViewEmployeeResult.Data.DOB != null)
                                        {
                                            empl.Dob = DateTime.ParseExact(response.ViewEmployeeResult.Data.DOB, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None);
                                        }
                                        if (response.ViewEmployeeResult.Data.DOJ != null)
                                        {
                                            empl.Doj = DateTime.ParseExact(response.ViewEmployeeResult.Data.DOJ, "dd/MM/yyyy", null);
                                        }
                                        empl.DataChange = response.ViewEmployeeResult.Data.DataChange;
                                        empl.Department = response.ViewEmployeeResult.Data.Department;
                                        empl.Designation = response.ViewEmployeeResult.Data.Designation;
                                        empl.Division = response.ViewEmployeeResult.Data.Division;
                                        empl.EmpType = response.ViewEmployeeResult.Data.EmpType;
                                        empl.EmploymentStatus = response.ViewEmployeeResult.Data.EmploymentStatus;
                                        empl.FunctionalManager = response.ViewEmployeeResult.Data.FunctionalManager;
                                        empl.FunctionalManagerCode = response.ViewEmployeeResult.Data.FunctionalManagerCode;
                                        empl.Gender = response.ViewEmployeeResult.Data.Gender;
                                        empl.Lwd = response.ViewEmployeeResult.Data.LWD;
                                        empl.Level = response.ViewEmployeeResult.Data.Level;
                                        empl.Manager = response.ViewEmployeeResult.Data.Manager;
                                        empl.ManagerCode = response.ViewEmployeeResult.Data.ManagerCode;
                                        empl.MiddleName = response.ViewEmployeeResult.Data.MiddleName;
                                        empl.MobileNo = response.ViewEmployeeResult.Data.MobileNo;
                                        empl.Name = response.ViewEmployeeResult.Data.Name;
                                        empl.NewTranId = response.ViewEmployeeResult.Data.NewTranID;
                                        empl.OfficeLocation = response.ViewEmployeeResult.Data.OfficeLocation;
                                        empl.OfficeLocationCity = response.ViewEmployeeResult.Data.OfficeLocationCity;
                                        if (response.ViewEmployeeResult.Data.OnDate != null)
                                        {
                                            empl.OnDate = DateTime.ParseExact(response.ViewEmployeeResult.Data.OnDate, "dd/MM/yyyy", null);
                                        }
                                        empl.Pin = response.ViewEmployeeResult.Data.Pin;
                                        if (response.ViewEmployeeResult.Data.SepSubmissionDate != null)
                                        {
                                            empl.SepSubmissionDate = DateTime.ParseExact(response.ViewEmployeeResult.Data.SepSubmissionDate, "dd/MM/yyyy", null);
                                        }
                                        empl.State = response.ViewEmployeeResult.Data.State;
                                        empl.Status = response.ViewEmployeeResult.Data.Status;
                                        empl.WorkLocation = response.ViewEmployeeResult.Data.WorkLocation;
                                        empl.WorkLocationCity = response.ViewEmployeeResult.Data.WorkLocationCity;
                                        empl.WorkNature = response.ViewEmployeeResult.Data.WorkNature;
                                        empl.EmpCode = response.ViewEmployeeResult.Data.EmpCode;
                                        if (response.ViewEmployeeResult.Data.CustomAdditional != null)
                                        {
                                            var CustomAdditionalJson = JsonConvert.SerializeObject(response.ViewEmployeeResult.Data.CustomAdditional);
                                            empl.CustomAdditional = CustomAdditionalJson;
                                        }
                                        if (response.ViewEmployeeResult.Data.CustomOfficial != null)
                                        {
                                            var CustomOfficialJson = JsonConvert.SerializeObject(response.ViewEmployeeResult.Data.CustomOfficial);
                                            empl.CustomOfficial = CustomOfficialJson;
                                        }
                                        if (response.ViewEmployeeResult.Data.CustomPersonal != null)
                                        {
                                            var CustomPersonalJson = JsonConvert.SerializeObject(response.ViewEmployeeResult.Data.CustomPersonal);
                                            empl.CustomPersonal = CustomPersonalJson;
                                        }

                                        _dbContext.EmployeeDetails.Add(empl);
                                        _dbContext.SaveChanges();

                                    }
                                }
                            
                        }

                    }

                }
                return View();
            }
            return View();
        }
        public async Task<ActionResult> saveMLUCAsync()
        {
            if (ModelState.IsValid)
            {
                using (var client = new HttpClient())
                {
                    for (int a = 0; a < 10; a++)
                    {
                        for (int b = 0; b < 10; b++)
                        {

                            var emp = new EmployeeData { APIKey = "32a75a44-c927-42fb-b8ec-8d0898880d90", CorpURL = "https://motivitylabs.com", EmpCode = "MLUC" + a + "" + b };
                                var Response = await client.PostAsJsonAsync("https://motivity.eazework.com/EazeAPI/Employee.svc/ViewEmployee", emp);
                                string empobj = await Response.Content.ReadAsStringAsync();
                                ViewEmployeeResultResponse response = JsonConvert.DeserializeObject<ViewEmployeeResultResponse>(empobj);
                                if (response.ViewEmployeeResult.ErrorCode == 0)
                                {
                                    if (response != null)
                                    {
                                        EmployeeDetail empl = new EmployeeDetail();
                                        empl.EmailId = response.ViewEmployeeResult.Data.EmailID;
                                        empl.FirstName = response.ViewEmployeeResult.Data.FirstName;
                                        empl.LastName = response.ViewEmployeeResult.Data.LastName;
                                        empl.Address = response.ViewEmployeeResult.Data.Address;
                                        empl.Address1 = response.ViewEmployeeResult.Data.Address1;
                                        empl.City = response.ViewEmployeeResult.Data.City;
                                        empl.Company = response.ViewEmployeeResult.Data.Company;
                                        empl.Country = response.ViewEmployeeResult.Data.Country;
                                        if (response.ViewEmployeeResult.Data.DOB != null)
                                        {
                                            empl.Dob = DateTime.ParseExact(response.ViewEmployeeResult.Data.DOB, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None);
                                        }
                                        if (response.ViewEmployeeResult.Data.DOJ != null)
                                        {
                                            empl.Doj = DateTime.ParseExact(response.ViewEmployeeResult.Data.DOJ, "dd/MM/yyyy", null);
                                        }
                                        empl.DataChange = response.ViewEmployeeResult.Data.DataChange;
                                        empl.Department = response.ViewEmployeeResult.Data.Department;
                                        empl.Designation = response.ViewEmployeeResult.Data.Designation;
                                        empl.Division = response.ViewEmployeeResult.Data.Division;
                                        empl.EmpType = response.ViewEmployeeResult.Data.EmpType;
                                        empl.EmploymentStatus = response.ViewEmployeeResult.Data.EmploymentStatus;
                                        empl.FunctionalManager = response.ViewEmployeeResult.Data.FunctionalManager;
                                        empl.FunctionalManagerCode = response.ViewEmployeeResult.Data.FunctionalManagerCode;
                                        empl.Gender = response.ViewEmployeeResult.Data.Gender;
                                        empl.Lwd = response.ViewEmployeeResult.Data.LWD;
                                        empl.Level = response.ViewEmployeeResult.Data.Level;
                                        empl.Manager = response.ViewEmployeeResult.Data.Manager;
                                        empl.ManagerCode = response.ViewEmployeeResult.Data.ManagerCode;
                                        empl.MiddleName = response.ViewEmployeeResult.Data.MiddleName;
                                        empl.MobileNo = response.ViewEmployeeResult.Data.MobileNo;
                                        empl.Name = response.ViewEmployeeResult.Data.Name;
                                        empl.NewTranId = response.ViewEmployeeResult.Data.NewTranID;
                                        empl.OfficeLocation = response.ViewEmployeeResult.Data.OfficeLocation;
                                        empl.OfficeLocationCity = response.ViewEmployeeResult.Data.OfficeLocationCity;
                                        if (response.ViewEmployeeResult.Data.OnDate != null)
                                        {
                                            empl.OnDate = DateTime.ParseExact(response.ViewEmployeeResult.Data.OnDate, "dd/MM/yyyy", null);
                                        }
                                        empl.Pin = response.ViewEmployeeResult.Data.Pin;
                                        if (response.ViewEmployeeResult.Data.SepSubmissionDate != null)
                                        {
                                            empl.SepSubmissionDate = DateTime.ParseExact(response.ViewEmployeeResult.Data.SepSubmissionDate, "dd/MM/yyyy", null);
                                        }
                                        empl.State = response.ViewEmployeeResult.Data.State;
                                        empl.Status = response.ViewEmployeeResult.Data.Status;
                                        empl.WorkLocation = response.ViewEmployeeResult.Data.WorkLocation;
                                        empl.WorkLocationCity = response.ViewEmployeeResult.Data.WorkLocationCity;
                                        empl.WorkNature = response.ViewEmployeeResult.Data.WorkNature;
                                        empl.EmpCode = response.ViewEmployeeResult.Data.EmpCode;
                                        if (response.ViewEmployeeResult.Data.CustomAdditional != null)
                                        {
                                            var CustomAdditionalJson = JsonConvert.SerializeObject(response.ViewEmployeeResult.Data.CustomAdditional);
                                            empl.CustomAdditional = CustomAdditionalJson;
                                        }
                                        if (response.ViewEmployeeResult.Data.CustomOfficial != null)
                                        {
                                            var CustomOfficialJson = JsonConvert.SerializeObject(response.ViewEmployeeResult.Data.CustomOfficial);
                                            empl.CustomOfficial = CustomOfficialJson;
                                        }
                                        if (response.ViewEmployeeResult.Data.CustomPersonal != null)
                                        {
                                            var CustomPersonalJson = JsonConvert.SerializeObject(response.ViewEmployeeResult.Data.CustomPersonal);
                                            empl.CustomPersonal = CustomPersonalJson;
                                        }

                                        _dbContext.EmployeeDetails.Add(empl);
                                        _dbContext.SaveChanges();

                                    }
                                }
                            }
                        

                    }

                }
                return View();
            }
            return View();
        }
        public async Task<ActionResult> saveMLUSAsync()
        {
            if (ModelState.IsValid)
            {
                using (var client = new HttpClient())
                {
                    for (int a = 0; a < 10; a++)
                    {
                        for (int b = 0; b < 10; b++)
                        {

                            var emp = new EmployeeData { APIKey = "32a75a44-c927-42fb-b8ec-8d0898880d90", CorpURL = "https://motivitylabs.com", EmpCode = "MLUS" + a + "" + b };
                                var Response = await client.PostAsJsonAsync("https://motivity.eazework.com/EazeAPI/Employee.svc/ViewEmployee", emp);
                                string empobj = await Response.Content.ReadAsStringAsync();
                                ViewEmployeeResultResponse response = JsonConvert.DeserializeObject<ViewEmployeeResultResponse>(empobj);
                                if (response.ViewEmployeeResult.ErrorCode == 0)
                                {
                                    if (response != null)
                                    {
                                        EmployeeDetail empl = new EmployeeDetail();
                                        empl.EmailId = response.ViewEmployeeResult.Data.EmailID;
                                        empl.FirstName = response.ViewEmployeeResult.Data.FirstName;
                                        empl.LastName = response.ViewEmployeeResult.Data.LastName;
                                        empl.Address = response.ViewEmployeeResult.Data.Address;
                                        empl.Address1 = response.ViewEmployeeResult.Data.Address1;
                                        empl.City = response.ViewEmployeeResult.Data.City;
                                        empl.Company = response.ViewEmployeeResult.Data.Company;
                                        empl.Country = response.ViewEmployeeResult.Data.Country;
                                        if (response.ViewEmployeeResult.Data.DOB != null)
                                        {
                                            empl.Dob = DateTime.ParseExact(response.ViewEmployeeResult.Data.DOB, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None);
                                        }
                                        if (response.ViewEmployeeResult.Data.DOJ != null)
                                        {
                                            empl.Doj = DateTime.ParseExact(response.ViewEmployeeResult.Data.DOJ, "dd/MM/yyyy", null);
                                        }
                                        empl.DataChange = response.ViewEmployeeResult.Data.DataChange;
                                        empl.Department = response.ViewEmployeeResult.Data.Department;
                                        empl.Designation = response.ViewEmployeeResult.Data.Designation;
                                        empl.Division = response.ViewEmployeeResult.Data.Division;
                                        empl.EmpType = response.ViewEmployeeResult.Data.EmpType;
                                        empl.EmploymentStatus = response.ViewEmployeeResult.Data.EmploymentStatus;
                                        empl.FunctionalManager = response.ViewEmployeeResult.Data.FunctionalManager;
                                        empl.FunctionalManagerCode = response.ViewEmployeeResult.Data.FunctionalManagerCode;
                                        empl.Gender = response.ViewEmployeeResult.Data.Gender;
                                        empl.Lwd = response.ViewEmployeeResult.Data.LWD;
                                        empl.Level = response.ViewEmployeeResult.Data.Level;
                                        empl.Manager = response.ViewEmployeeResult.Data.Manager;
                                        empl.ManagerCode = response.ViewEmployeeResult.Data.ManagerCode;
                                        empl.MiddleName = response.ViewEmployeeResult.Data.MiddleName;
                                        empl.MobileNo = response.ViewEmployeeResult.Data.MobileNo;
                                        empl.Name = response.ViewEmployeeResult.Data.Name;
                                        empl.NewTranId = response.ViewEmployeeResult.Data.NewTranID;
                                        empl.OfficeLocation = response.ViewEmployeeResult.Data.OfficeLocation;
                                        empl.OfficeLocationCity = response.ViewEmployeeResult.Data.OfficeLocationCity;
                                        if (response.ViewEmployeeResult.Data.OnDate != null)
                                        {
                                            empl.OnDate = DateTime.ParseExact(response.ViewEmployeeResult.Data.OnDate, "dd/MM/yyyy", null);
                                        }
                                        empl.Pin = response.ViewEmployeeResult.Data.Pin;
                                        if (response.ViewEmployeeResult.Data.SepSubmissionDate != null)
                                        {
                                            empl.SepSubmissionDate = DateTime.ParseExact(response.ViewEmployeeResult.Data.SepSubmissionDate, "dd/MM/yyyy", null);
                                        }
                                        empl.State = response.ViewEmployeeResult.Data.State;
                                        empl.Status = response.ViewEmployeeResult.Data.Status;
                                        empl.WorkLocation = response.ViewEmployeeResult.Data.WorkLocation;
                                        empl.WorkLocationCity = response.ViewEmployeeResult.Data.WorkLocationCity;
                                        empl.WorkNature = response.ViewEmployeeResult.Data.WorkNature;
                                        empl.EmpCode = response.ViewEmployeeResult.Data.EmpCode;
                                        if (response.ViewEmployeeResult.Data.CustomAdditional != null)
                                        {
                                            var CustomAdditionalJson = JsonConvert.SerializeObject(response.ViewEmployeeResult.Data.CustomAdditional);
                                            empl.CustomAdditional = CustomAdditionalJson;
                                        }
                                        if (response.ViewEmployeeResult.Data.CustomOfficial != null)
                                        {
                                            var CustomOfficialJson = JsonConvert.SerializeObject(response.ViewEmployeeResult.Data.CustomOfficial);
                                            empl.CustomOfficial = CustomOfficialJson;
                                        }
                                        if (response.ViewEmployeeResult.Data.CustomPersonal != null)
                                        {
                                            var CustomPersonalJson = JsonConvert.SerializeObject(response.ViewEmployeeResult.Data.CustomPersonal);
                                            empl.CustomPersonal = CustomPersonalJson;
                                        }

                                        _dbContext.EmployeeDetails.Add(empl);
                                        _dbContext.SaveChanges();

                                    }
                                }
                            }
                        

                    }

                }
                return View();
            }
            return View();
        }
        public async Task<ActionResult> CreateAsync(IFormCollection collection)
        {
            if (ModelState.IsValid)
            {
                using (var client = new HttpClient())
                {
                    for (int a = 0; a < 8; a++)
                    {
                        for (int b = 0; b < 10; b++)
                        {
                            for (int c = 0; c < 10; c++)
                            {
                                
                                var emp = new EmployeeData { APIKey = "32a75a44-c927-42fb-b8ec-8d0898880d90", CorpURL = "https://motivitylabs.com", EmpCode = "MLI" + a + "" + b + "" + c };
                                var Response = await client.PostAsJsonAsync("https://motivity.eazework.com/EazeAPI/Employee.svc/ViewEmployee", emp);
                                string empobj = await Response.Content.ReadAsStringAsync();
                                ViewEmployeeResultResponse response = JsonConvert.DeserializeObject<ViewEmployeeResultResponse>(empobj);
                                if (response.ViewEmployeeResult.ErrorCode == 0)
                                {
                                    if (response != null)
                                    {
                                        EmployeeDetail empl = new EmployeeDetail();
                                        empl.EmailId = response.ViewEmployeeResult.Data.EmailID;
                                        empl.FirstName = response.ViewEmployeeResult.Data.FirstName;
                                        empl.LastName = response.ViewEmployeeResult.Data.LastName;
                                        empl.Address = response.ViewEmployeeResult.Data.Address;
                                        empl.Address1 = response.ViewEmployeeResult.Data.Address1;
                                        empl.City = response.ViewEmployeeResult.Data.City;
                                        empl.Company = response.ViewEmployeeResult.Data.Company;
                                        empl.Country = response.ViewEmployeeResult.Data.Country;
                                        if (response.ViewEmployeeResult.Data.DOB != null)
                                        {
                                            empl.Dob = DateTime.ParseExact(response.ViewEmployeeResult.Data.DOB, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None);
                                        }
                                        if (response.ViewEmployeeResult.Data.DOJ != null)
                                        {
                                            empl.Doj = DateTime.ParseExact(response.ViewEmployeeResult.Data.DOJ, "dd/MM/yyyy", null);
                                        }
                                        empl.DataChange = response.ViewEmployeeResult.Data.DataChange;
                                        empl.Department = response.ViewEmployeeResult.Data.Department;
                                        empl.Designation = response.ViewEmployeeResult.Data.Designation;
                                        empl.Division = response.ViewEmployeeResult.Data.Division;
                                        empl.EmpType = response.ViewEmployeeResult.Data.EmpType;
                                        empl.EmploymentStatus = response.ViewEmployeeResult.Data.EmploymentStatus;
                                        empl.FunctionalManager = response.ViewEmployeeResult.Data.FunctionalManager;
                                        empl.FunctionalManagerCode = response.ViewEmployeeResult.Data.FunctionalManagerCode;
                                        empl.Gender = response.ViewEmployeeResult.Data.Gender;
                                        empl.Lwd = response.ViewEmployeeResult.Data.LWD;
                                        empl.Level = response.ViewEmployeeResult.Data.Level;
                                        empl.Manager = response.ViewEmployeeResult.Data.Manager;
                                        empl.ManagerCode = response.ViewEmployeeResult.Data.ManagerCode;
                                        empl.MiddleName = response.ViewEmployeeResult.Data.MiddleName;
                                        empl.MobileNo = response.ViewEmployeeResult.Data.MobileNo;
                                        empl.Name = response.ViewEmployeeResult.Data.Name;
                                        empl.NewTranId = response.ViewEmployeeResult.Data.NewTranID;
                                        empl.OfficeLocation = response.ViewEmployeeResult.Data.OfficeLocation;
                                        empl.OfficeLocationCity = response.ViewEmployeeResult.Data.OfficeLocationCity;
                                        if (response.ViewEmployeeResult.Data.OnDate != null)
                                        {
                                            empl.OnDate = DateTime.ParseExact(response.ViewEmployeeResult.Data.OnDate, "dd/MM/yyyy", null);
                                        }
                                        empl.Pin = response.ViewEmployeeResult.Data.Pin;
                                        if (response.ViewEmployeeResult.Data.SepSubmissionDate != null)
                                        {
                                            empl.SepSubmissionDate = DateTime.ParseExact(response.ViewEmployeeResult.Data.SepSubmissionDate, "dd/MM/yyyy", null);
                                        }
                                        empl.State = response.ViewEmployeeResult.Data.State;
                                        empl.Status = response.ViewEmployeeResult.Data.Status;
                                        empl.WorkLocation = response.ViewEmployeeResult.Data.WorkLocation;
                                        empl.WorkLocationCity = response.ViewEmployeeResult.Data.WorkLocationCity;
                                        empl.WorkNature = response.ViewEmployeeResult.Data.WorkNature;
                                        empl.EmpCode = response.ViewEmployeeResult.Data.EmpCode;
                                        if (response.ViewEmployeeResult.Data.CustomAdditional != null)
                                        {
                                            var CustomAdditionalJson = JsonConvert.SerializeObject(response.ViewEmployeeResult.Data.CustomAdditional);
                                            empl.CustomAdditional = CustomAdditionalJson;
                                        }
                                        if (response.ViewEmployeeResult.Data.CustomOfficial != null)
                                        {
                                            var CustomOfficialJson = JsonConvert.SerializeObject(response.ViewEmployeeResult.Data.CustomOfficial);
                                            empl.CustomOfficial = CustomOfficialJson;
                                        }
                                        if (response.ViewEmployeeResult.Data.CustomPersonal != null)
                                        {
                                            var CustomPersonalJson = JsonConvert.SerializeObject(response.ViewEmployeeResult.Data.CustomPersonal);
                                            empl.CustomPersonal = CustomPersonalJson;
                                        }

                                        _dbContext.EmployeeDetails.Add(empl);
                                        _dbContext.SaveChanges();

                                    }
                                }
                            }
                        }

                    }
                      
                }
                return View();
            }
            return View();
        }
        public async Task<IActionResult> ExportAsync()

        {
            List<string> _userSlectedPropertiesList = SessionHelper.GetObjectFromJson<List<string>>(HttpContext.Session, "userSelectedProps");
            string sWebRootFolder = _hostingEnvironment.WebRootPath;
            //string sWebRootFolder = @"C:\Files\";
            string datetime = DateTime.Now.ToString().Replace(":","_");
            string sFileName = @"MLEmployeesReport_"+ datetime+".xlsx";

            string URL = string.Format("{0}://{1}/{2}", Request.Scheme, Request.Host, sFileName);

            FileInfo file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));

            var memory = new MemoryStream();

            using (var fs = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Create, FileAccess.Write))

            {
                if (_userSlectedPropertiesList.Count > 0)
                {
                    List<EmployeeDetail> empList = _dbContext.EmployeeDetails.ToList();
                    IWorkbook workbook;

                    workbook = new XSSFWorkbook();

                    ISheet excelSheet = workbook.CreateSheet("employee");

                    IRow row = excelSheet.CreateRow(0);
                    int k = 0;
                    foreach(string props in _userSlectedPropertiesList){

                        if (props == "EmployeeCode")
                        {
                            row.CreateCell(k).SetCellValue("Employee Code");
                        }
                        if (props == "EmployeeName")
                        {
                            row.CreateCell(k).SetCellValue("Employee Name");
  
                        }
                        if (props == "ProjectName")
                        {
                            row.CreateCell(k).SetCellValue("Project Name");
                           
                        }
                        if (props == "Designation")
                        {
                            row.CreateCell(k).SetCellValue("Designation");
                        }
                        if (props == "Department")
                        {
                            row.CreateCell(k).SetCellValue("Department");
                        }
                        if (props == "SubDepartment")
                        {
                            row.CreateCell(k).SetCellValue("Sub - Department");
                        }
                        if (props == "WorkLocation")
                        {
                            row.CreateCell(k).SetCellValue("Work Location");
                        }
                        if (props == "Level")
                        {
                            row.CreateCell(k).SetCellValue("Level");
                        }
                        if (props == "Email")
                        {
                            row.CreateCell(k).SetCellValue("Email");
                        }
                        if (props == "ManagerEmail")
                        {
                            row.CreateCell(k).SetCellValue("Manager Email");
                       
                        }
                        if (props == "CountryCode")
                        {
                            row.CreateCell(k).SetCellValue("Country Code");


                        }
                        if (props == "Mobile")
                        {

                            row.CreateCell(k).SetCellValue("Mobile");
                            
                        }
                        if (props == "DateOfJoining")
                        {
                            row.CreateCell(k).SetCellValue("Date Of Joining");
                          

                        }
                        if (props == "Gender")
                        {
                            row.CreateCell(k).SetCellValue("Gender");
                        }
                        if (props == "EmployeeType")
                        {
                            row.CreateCell(k).SetCellValue("Employee Type");
                        }                    
                        if (props == "EmploymentStatus")
                        {
                            row.CreateCell(k).SetCellValue("Employment Status");
                        }
                        if (props == "DeskNo")
                        {
                            row.CreateCell(k).SetCellValue("Desk No");
                        }
                        if (props == "PrevExp")
                        {
                            row.CreateCell(k).SetCellValue("Prev Exp");
                          
                        }
                        if (props == "CurrentExperience")
                        {
                            row.CreateCell(k).SetCellValue("Current Experience");

                        }
                        if (props == "TotalExperience")
                        {
                            row.CreateCell(k).SetCellValue("Total Experience");
                        
                        }
                        if (props == "Billing")
                        {
                            row.CreateCell(k).SetCellValue("Billing");
                            
                        }
                        if (props == "CoreSkills")
                        {
                            row.CreateCell(k).SetCellValue("Core Skills (>=7/10 rating)");
                        }
                        if (props == "MinorSkills")
                        {

                                row.CreateCell(k).SetCellValue("Minor Skills (<=6/10 rating)");
                               
                        }

                        if (props == "Certifications")
                        {
                            row.CreateCell(k).SetCellValue("Certifications(include certification org)");
                        }
                        if (props == "Customer")
                        {
                            row.CreateCell(k).SetCellValue("Customer");
                        }
                        k++;
                    }
                       
                    


                    int j = 1;
                    foreach (EmployeeDetail e in empList)
                    {

                        IRow rows = excelSheet.CreateRow(j);
                        int i = 0;
                        List<CustomAdditional> customAdditional = JsonConvert.DeserializeObject<List<CustomAdditional>>(e.CustomAdditional);
                        List<CustomOfficial> customOfficial = JsonConvert.DeserializeObject<List<CustomOfficial>>(e.CustomOfficial);
                        foreach (string props in _userSlectedPropertiesList)
                        {
                            if (props == "EmployeeCode")
                            {
                                rows.CreateCell(i).SetCellValue(e.EmpCode);
                            }
                            if (props == "EmployeeName")
                            {
                                rows.CreateCell(i).SetCellValue(e.FirstName + " " + e.MiddleName + e.LastName);

                            }
                            if (props == "ProjectName")
                            {
                                if (customOfficial.Count > 2)
                                {
                                    rows.CreateCell(i).SetCellValue(customOfficial[2].FieldValue);
                                }
                                else { rows.CreateCell(i).SetCellValue(""); }

                            }
                            if (props == "Designation")
                            {
                                rows.CreateCell(i).SetCellValue(e.Designation);

                               
                            }
                            if (props == "Department")
                            {
                                rows.CreateCell(i).SetCellValue(e.Department);
                                
                            }
                            if (props == "SubDepartment")
                            {
                                rows.CreateCell(i).SetCellValue("");
                                
                            }
                            if (props == "WorkLocation")
                            {
                                rows.CreateCell(i).SetCellValue(e.WorkLocation);

                                
                            }
                            if (props == "Level")
                            {
                                rows.CreateCell(i).SetCellValue(e.Level);

                              
                            }
                            if (props == "Email")
                            {
                                rows.CreateCell(i).SetCellValue(e.EmailId);
                            }
                            if (props == "ManagerEmail")
                            {
                                if (!string.IsNullOrEmpty(e.ManagerCode))
                                {
                                    EmployeeDetail ed = empList.Find(s => s.EmpCode == e.ManagerCode);
                                    rows.CreateCell(i).SetCellValue(ed.EmailId);
                                }
                                else{ rows.CreateCell(i).SetCellValue(""); }
                            }
                            if (props == "CountryCode")
                            {
                                if (e.EmpCode.Contains("MLI"))
                                {
                                    rows.CreateCell(i).SetCellValue("91");
                                }
                                else
                                {
                                    rows.CreateCell(i).SetCellValue("01");
                                }
                            }
                            if (props == "Mobile")
                            {

                                rows.CreateCell(i).SetCellValue(e.MobileNo);
                                
                            }
                            if (props == "DateOfJoining")
                            {
                                rows.CreateCell(i).SetCellValue(e.Doj.ToString().Substring(0, 10));
                            }
                            if (props == "Gender")
                            {
                                rows.CreateCell(i).SetCellValue(e.Gender);       
                            }
                            if (props == "EmployeeType")
                            {
                                rows.CreateCell(i).SetCellValue(e.EmpType);    
                            }
                            if (props == "EmploymentStatus")
                            {
                                rows.CreateCell(i).SetCellValue(e.EmploymentStatus);
                            }
                            if (props == "DeskNo")
                            {
                                rows.CreateCell(i).SetCellValue("");     
                            }
                            if (props == "PrevExp")
                            {
                                if (customAdditional.Count > 0)
                                {
                                    rows.CreateCell(i).SetCellValue(customAdditional[0].FieldValue);
                                }
                                else {
                                    rows.CreateCell(i).SetCellValue("");
                                }

                            }
                            if (props == "CurrentExperience")
                            {
                                DateTime d1 = DateTime.Now;
                                DateTime d2 = DateTime.ParseExact(e.Doj.ToString(), "dd-MM-yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None);
                                rows.CreateCell(i).SetCellValue(new DateDifference(d1, d2).ToString());
                            }
                            if (props == "TotalExperience")
                            {
                                rows.CreateCell(i).SetCellValue("");    
                            }
                            if (props == "Billing")
                            {
                                if (customOfficial.Count > 0)
                                {
                                    rows.CreateCell(i).SetCellValue(customOfficial[0].FieldValue);
                                }
                                else {
                                    rows.CreateCell(i).SetCellValue("");
                                }
                            }
                            if (props == "CoreSkills")
                            {

                                CustomAdditional coreskill = customAdditional.Find(s => s.FieldName == "Core Skills (>=7/10 rating)");
                                if (coreskill != null)
                                {
                                    rows.CreateCell(i).SetCellValue(coreskill.FieldValue);
                                }
                                else { rows.CreateCell(i).SetCellValue(""); }
                            }
                            if (props == "MinorSkills")
                            {

                                CustomAdditional minorskill = customAdditional.Find(s => s.FieldName == "Minor Skills (<=6/10 rating)");

                                if (minorskill != null)
                                {
                                    rows.CreateCell(i).SetCellValue(minorskill.FieldValue);
                                }
                                else{ rows.CreateCell(i).SetCellValue(""); }
                            }

                            if (props == "Certifications")
                            {
                                CustomAdditional cerfication = customAdditional.Find(s => s.FieldName == "Certifications(include certification org)");
                                if (cerfication != null)
                                {
                                    rows.CreateCell(i).SetCellValue(cerfication.FieldValue);
                                }
                                else{ rows.CreateCell(i).SetCellValue(""); }

                                
                            }
                            if (props == "Customer")
                            {
                                rows.CreateCell(i).SetCellValue(customOfficial[1].FieldValue);
                            }
                            i++;
                            

                           

                           

                          

                           

                           
                           
                            
                           

                           
                        }

                        j++;


                    }
                    workbook.Write(fs);

                    using (var stream = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Open))
                    {
                        await stream.CopyToAsync(memory);
                    }
                    memory.Position = 0;
                    return File(memory, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", sFileName);
                }
                else
                {
                    return RedirectToAction(actionName: "EmployeesList", controllerName: "Employee");

                }
            }


        }


        // GET: EmployeeController/Delete/5
        public async Task<RedirectToActionResult> getNewEmployeeListAsync()
        {
            if (ModelState.IsValid)
            {
                using (var client = new HttpClient())
                {
                    List<EmployeeSyncing> lisEmpSync = _dbContext.EmployeeSyncing.ToList();

                    if (lisEmpSync.Count > 0)
                    {
                        DateTime dfrom = DateTime.ParseExact(lisEmpSync[lisEmpSync.Count - 1].SyncedOn.ToString(), "dd-MM-yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None);
                        string fromDate = dfrom.ToShortDateString().Replace("-", "/");
                        DateTime dTo = dfrom.AddDays(6);
                        string toDate = dTo.ToShortDateString().Replace("-", "/");

                        string date = DateTime.Now.ToShortDateString();
                        var emp = new NewEmployeeListRequest { APIKey = "32a75a44-c927-42fb-b8ec-8d0898880d90", CorpURL = "https://motivitylabs.com", DateFrom = fromDate, DateTo = toDate };
                        var Response = await client.PostAsJsonAsync("https://motivity.eazework.com/EazeAPI/Employee.svc/NewEmployeeList", emp);
                        string empobj = await Response.Content.ReadAsStringAsync();
                        NewEmployeeListResultResponse response = JsonConvert.DeserializeObject<NewEmployeeListResultResponse>(empobj);
                        if (response.NewEmployeeListResult.ErrorCode == 0)
                        {
                            if (response != null)
                            {
                                if (response.NewEmployeeListResult.Data.Count > 0)
                                {
                                    int dbchanges = 0;
                                    foreach (Data empdata in response.NewEmployeeListResult.Data)
                                    {
                                        List<EmployeeDetail> empList = _dbContext.EmployeeDetails.ToList();
                                        EmployeeDetail ed = empList.Find(s => s.EmpCode == empdata.EmpCode);
                                        if (ed == null)
                                        {
                                            EmployeeDetail empl = new EmployeeDetail();
                                            empl.EmailId = empdata.EmailID;
                                            empl.FirstName = empdata.FirstName;
                                            empl.LastName = empdata.LastName;
                                            empl.Address = empdata.Address;
                                            empl.Address1 = empdata.Address1;
                                            empl.City = empdata.City;
                                            empl.Company = empdata.Company;
                                            empl.Country = empdata.Country;
                                            if (empdata.DOB != null)
                                            {
                                                empl.Dob = DateTime.ParseExact(empdata.DOB, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None);
                                            }
                                            if (empdata.DOJ != null)
                                            {
                                                empl.Doj = DateTime.ParseExact(empdata.DOJ, "dd/MM/yyyy", null);
                                            }
                                            empl.DataChange = empdata.DataChange;
                                            empl.Department = empdata.Department;
                                            empl.Designation = empdata.Designation;
                                            empl.Division = empdata.Division;
                                            empl.EmpType = empdata.EmpType;
                                            empl.EmploymentStatus = empdata.EmploymentStatus;
                                            empl.FunctionalManager = empdata.FunctionalManager;
                                            empl.FunctionalManagerCode = empdata.FunctionalManagerCode;
                                            empl.Gender = empdata.Gender;
                                            empl.Lwd = empdata.LWD;
                                            empl.Level = empdata.Level;
                                            empl.Manager = empdata.Manager;
                                            empl.ManagerCode = empdata.ManagerCode;
                                            empl.MiddleName = empdata.MiddleName;
                                            empl.MobileNo = empdata.MobileNo;
                                            empl.Name = empdata.Name;
                                            empl.NewTranId = empdata.NewTranID;
                                            empl.OfficeLocation = empdata.OfficeLocation;
                                            empl.OfficeLocationCity = empdata.OfficeLocationCity;
                                            if (empdata.OnDate != null)
                                            {
                                                empl.OnDate = DateTime.ParseExact(empdata.OnDate, "dd/MM/yyyy", null);
                                            }
                                            empl.Pin = empdata.Pin;
                                            if (empdata.SepSubmissionDate != null)
                                            {
                                                empl.SepSubmissionDate = DateTime.ParseExact(empdata.SepSubmissionDate, "dd/MM/yyyy", null);
                                            }
                                            empl.State = empdata.State;
                                            empl.Status = empdata.Status;
                                            empl.WorkLocation = empdata.WorkLocation;
                                            empl.WorkLocationCity = empdata.WorkLocationCity;
                                            empl.WorkNature = empdata.WorkNature;
                                            empl.EmpCode = empdata.EmpCode;
                                            if (empdata.CustomAdditional != null)
                                            {
                                                var CustomAdditionalJson = JsonConvert.SerializeObject(empdata.CustomAdditional);
                                                empl.CustomAdditional = CustomAdditionalJson;
                                            }
                                            if (empdata.CustomOfficial != null)
                                            {
                                                var CustomOfficialJson = JsonConvert.SerializeObject(empdata.CustomOfficial);
                                                empl.CustomOfficial = CustomOfficialJson;
                                            }
                                            if (empdata.CustomPersonal != null)
                                            {
                                                var CustomPersonalJson = JsonConvert.SerializeObject(empdata.CustomPersonal);
                                                empl.CustomPersonal = CustomPersonalJson;
                                            }

                                            _dbContext.EmployeeDetails.Add(empl);
                                            dbchanges = _dbContext.SaveChanges();

                                        }
                                        else
                                        {
                                        }
                                    }
                                    if (dbchanges > 0)
                                    {
                                        EmployeeSyncing es = new EmployeeSyncing();
                                        es.SyncedOn = DateTime.ParseExact(DateTime.Now.ToString(), "dd-MM-yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None);
                                        es.SyncedBy = "Test";
                                        _dbContext.EmployeeSyncing.Add(es);
                                        _dbContext.SaveChanges();
                                    }


                                }
                            }


                        }


                    }

                }
                return RedirectToAction(actionName: "EmployeesList", controllerName: "Employee");
            }
            return RedirectToAction(actionName: "EmployeesList", controllerName: "Employee");

        }
        public async Task<ActionResult> updatedEmployeeListAsync()
        {


            if (ModelState.IsValid)
            {
                using (var client = new HttpClient())
                {
                    List<EmployeeSyncing> lisEmpSync = _dbContext.EmployeeSyncing.ToList();
                    if (lisEmpSync.Count > 0)
                    {
                        DateTime dfrom = DateTime.ParseExact(lisEmpSync[lisEmpSync.Count - 1].SyncedOn.ToString(), "dd-MM-yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None);
                        //DateTime dfrom = DateTime.ParseExact(DateTime.Now.ToString(), "dd-MM-yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None);
                        string fromDate = dfrom.ToShortDateString().Replace("-", "/");
                        DateTime dTo = dfrom.AddDays(6);
                        string toDate = dTo.ToShortDateString().Replace("-", "/");
                    
                    string date = DateTime.Now.ToShortDateString();
                    // var emp = new NewEmployeeListRequest { APIKey = "32a75a44-c927-42fb-b8ec-8d0898880d90", CorpURL = "https://motivitylabs.com", DateFrom = "28/10/2019", DateTo = "03/11/2019" };
                    var emp = new NewEmployeeListRequest { APIKey = "32a75a44-c927-42fb-b8ec-8d0898880d90", CorpURL = "https://motivitylabs.com", DateFrom = fromDate, DateTo = toDate };
                    var Response = await client.PostAsJsonAsync("https://motivity.eazework.com/EazeAPI/Employee.svc/UpdatedEmployeeList", emp);
                    string empobj = await Response.Content.ReadAsStringAsync();
                    UpdatedEmployeeListResultResponse response = JsonConvert.DeserializeObject<UpdatedEmployeeListResultResponse>(empobj);
                    if (response.UpdatedEmployeeListResult.ErrorCode == 0)
                    {
                        if (response != null)
                        {
                            if (response.UpdatedEmployeeListResult.Data.Count > 0)
                            {
                                int dbchanges = 0;
                                foreach (Data empdata in response.UpdatedEmployeeListResult.Data)
                                {
                                    List<EmployeeDetail> empList = _dbContext.EmployeeDetails.ToList();
                                    EmployeeDetail ed = empList.Find(s => s.EmpCode == empdata.EmpCode);
                                    if (ed != null)
                                    {

                                        //EmployeeDetail empl = new EmployeeDetail();
                                        if (empdata.EmailID != null)
                                        {
                                            ed.EmailId = empdata.EmailID;
                                        }
                                        if (empdata.FirstName != null)
                                        {
                                            ed.FirstName = empdata.FirstName;
                                        }
                                        if (empdata.LastName != null)
                                        {
                                            ed.LastName = empdata.LastName;
                                        }
                                        if (empdata.Address != null)
                                        {
                                            ed.Address = empdata.Address;
                                        }
                                        if (empdata.Address1 != null)
                                        {
                                            ed.Address1 = empdata.Address1;
                                        }
                                        if (empdata.City != null)
                                        {
                                            ed.City = empdata.City;
                                        }
                                        if (empdata.Company != null)
                                        {
                                            ed.Company = empdata.Company;
                                        }
                                        if (empdata.Country != null)
                                        {
                                            ed.Country = empdata.Country;
                                        }
                                        if (!string.IsNullOrEmpty(empdata.DOB))
                                        {
                                            ed.Dob = DateTime.ParseExact(empdata.DOB, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None);
                                        }
                                        if (empdata.DOJ != null)
                                        {
                                            ed.Doj = DateTime.ParseExact(empdata.DOJ, "dd/MM/yyyy", null);
                                        }
                                        if (empdata.DataChange != null)
                                        {
                                            ed.DataChange = empdata.DataChange;
                                        }
                                        if (empdata.Department != null)
                                        {
                                            ed.Department = empdata.Department;
                                        }
                                        if (empdata.Designation != null)
                                        {
                                            ed.Designation = empdata.Designation;
                                        }
                                        if (empdata.Division != null)
                                        {
                                            ed.Division = empdata.Division;
                                        }
                                        if (empdata.EmpType != null)
                                        {
                                            ed.EmpType = empdata.EmpType;
                                        }
                                        if (empdata.EmploymentStatus != null)
                                        {
                                            ed.EmploymentStatus = empdata.EmploymentStatus;
                                        }
                                        if (empdata.FunctionalManager != null)
                                        {
                                            ed.FunctionalManager = empdata.FunctionalManager;
                                        }
                                        if (empdata.FunctionalManagerCode != null)
                                        {
                                            ed.FunctionalManagerCode = empdata.FunctionalManagerCode;
                                        }
                                        if (empdata.Gender != null)
                                        {
                                            ed.Gender = empdata.Gender;
                                        }
                                        if (empdata.LWD != null)
                                        {
                                            ed.Lwd = empdata.LWD;
                                        }
                                        if (empdata.Level != null)
                                        {
                                            ed.Level = empdata.Level;
                                        }
                                        if (empdata.Manager != null)
                                        {
                                            ed.Manager = empdata.Manager;
                                        }
                                        if (empdata.ManagerCode != null)
                                        {
                                            ed.ManagerCode = empdata.ManagerCode;
                                        }
                                        if (empdata.MiddleName != null)
                                        {
                                            ed.MiddleName = empdata.MiddleName;
                                        }
                                        if (empdata.MobileNo != null)
                                        {
                                            ed.MobileNo = empdata.MobileNo;
                                        }
                                        if (empdata.Name != null)
                                        {
                                            ed.Name = empdata.Name;
                                        }
                                        if (empdata.NewTranID != null)
                                        {
                                            ed.NewTranId = empdata.NewTranID;
                                        }
                                        if (empdata.OfficeLocation != null)
                                        {
                                            ed.OfficeLocation = empdata.OfficeLocation;
                                        }
                                        if (empdata.OfficeLocationCity != null)
                                        {
                                            ed.OfficeLocationCity = empdata.OfficeLocationCity;
                                        }
                                        if (empdata.OnDate != null)
                                        {
                                            ed.OnDate = DateTime.ParseExact(empdata.OnDate, "dd/MM/yyyy", null);
                                        }
                                        if (empdata.Pin != null)
                                        {
                                            ed.Pin = empdata.Pin;
                                        }
                                        if (empdata.SepSubmissionDate != null)
                                        {
                                            ed.SepSubmissionDate = DateTime.ParseExact(empdata.SepSubmissionDate, "dd/MM/yyyy", null);
                                        }
                                        if (empdata.State != null)
                                        {
                                            ed.State = empdata.State;
                                        }
                                        if (empdata.Status != null)
                                        {
                                            ed.Status = empdata.Status;
                                        }
                                        if (empdata.WorkLocation != null)
                                        {
                                            ed.WorkLocation = empdata.WorkLocation;
                                        }
                                        if (empdata.WorkLocationCity != null)
                                        {
                                            ed.WorkLocationCity = empdata.WorkLocationCity;
                                        }
                                        if (empdata.WorkNature != null)
                                        {
                                            ed.WorkNature = empdata.WorkNature;
                                        }
                                        if (empdata.EmpCode != null)
                                        {
                                            ed.EmpCode = empdata.EmpCode;
                                        }
                                        if (empdata.CustomAdditional != null && empdata.CustomAdditional.Count > 0)
                                        {
                                            List<CustomAdditionals> customAdditionalAPi = empdata.CustomAdditional;
                                            List<CustomAdditional> customAdditionalLocal = JsonConvert.DeserializeObject<List<CustomAdditional>>(ed.CustomAdditional);
                                            List<CustomAdditional> customAdditionalTepm = new List<CustomAdditional>();
                                            foreach (CustomAdditional cd in customAdditionalLocal)
                                            {
                                                CustomAdditional cd3 = new CustomAdditional();
                                                CustomAdditionals cadl = customAdditionalAPi.Find(s => s.FieldName == cd.FieldName);
                                                if (cadl != null)
                                                {
                                                    cd3.FieldName = cadl.FieldName;
                                                    cd3.FieldValue = cadl.FieldValue;
                                                }
                                                else
                                                {
                                                    cd3.FieldName = cd.FieldName;
                                                    cd3.FieldValue = cd.FieldValue;
                                                }
                                                customAdditionalTepm.Add(cd3);

                                            }
                                            var CustomAdditionalJson = JsonConvert.SerializeObject(customAdditionalTepm);
                                            ed.CustomAdditional = CustomAdditionalJson;

                                        }
                                        if (empdata.CustomOfficial != null && empdata.CustomOfficial.Count > 0)
                                        {
                                            List<CustomOfficials> customOfficialAPI = empdata.CustomOfficial;
                                            List<CustomOfficial> customOfficialLocal = JsonConvert.DeserializeObject<List<CustomOfficial>>(ed.CustomOfficial);
                                            List<CustomOfficial> customOfficialTepm = new List<CustomOfficial>();
                                            foreach (CustomOfficial cd in customOfficialLocal)
                                            {
                                                CustomOfficial cd3 = new CustomOfficial();
                                                CustomOfficials cadl = customOfficialAPI.Find(s => s.FieldName == cd.FieldName);
                                                if (cadl != null)
                                                {
                                                    cd3.FieldName = cadl.FieldName;
                                                    cd3.FieldValue = cadl.FieldValue;
                                                }
                                                else
                                                {
                                                    cd3.FieldName = cd.FieldName;
                                                    cd3.FieldValue = cd.FieldValue;
                                                }
                                                customOfficialTepm.Add(cd3);

                                            }
                                            var CustomOfficialJson = JsonConvert.SerializeObject(customOfficialTepm);
                                            ed.CustomAdditional = CustomOfficialJson;
                                        }
                                        if (empdata.CustomPersonal != null && empdata.CustomPersonal.Count > 0)
                                        {
                                            List<CustomPersonals> customPersonalAPI = empdata.CustomPersonal;
                                            List<CustomPersonals> customPersonalLocal = JsonConvert.DeserializeObject<List<CustomPersonals>>(ed.CustomPersonal);

                                            List<CustomPersonals> customPersonalTepm = new List<CustomPersonals>();
                                            foreach (CustomPersonals cd in customPersonalLocal)
                                            {
                                                CustomPersonals cd3 = new CustomPersonals();
                                                CustomPersonals cadl = customPersonalAPI.Find(s => s.FieldName == cd.FieldName);
                                                if (cadl != null)
                                                {
                                                    cd3.FieldName = cadl.FieldName;
                                                    cd3.FieldValue = cadl.FieldValue;
                                                }
                                                else
                                                {
                                                    cd3.FieldName = cd.FieldName;
                                                    cd3.FieldValue = cd.FieldValue;
                                                }
                                                customPersonalTepm.Add(cd3);

                                            }
                                            var CustomPersonalJson = JsonConvert.SerializeObject(customPersonalTepm);
                                            ed.CustomPersonal = CustomPersonalJson;

                                        }

                                        _dbContext.EmployeeDetails.Update(ed);
                                        dbchanges = _dbContext.SaveChanges();

                                    }
                                    else
                                    {
                                    }

                                }
                                if (dbchanges > 0)
                                {
                                    EmployeeSyncing es = new EmployeeSyncing();
                                    es.SyncedOn = DateTime.ParseExact(DateTime.Now.ToString(), "dd-MM-yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None);
                                    es.SyncedBy = "Test";
                                    _dbContext.EmployeeSyncing.Add(es);
                                    _dbContext.SaveChanges();
                                }


                            }


                        }


                    }
                }

                }
                return RedirectToAction(actionName: "EmployeesList", controllerName: "Employee");
            }
            return RedirectToAction(actionName: "EmployeesList", controllerName: "Employee");
        }
        [HttpPost]
        public void UserSelectedProperties(IFormCollection collections)
        {
            if (collections.Count > 0)
            {

                
            }
        }
        public async Task<ActionResult> SyncDataAsync() {
            // getNewEmployeeListAsync();
            using (var client = new HttpClient())
            {
                List<EmployeeSyncing> lisEmpSync = _dbContext.EmployeeSyncing.ToList();

                if (lisEmpSync.Count > 0)
                {
                    DateTime dfrom = DateTime.ParseExact(lisEmpSync[lisEmpSync.Count - 1].SyncedOn.ToString(), "dd-MM-yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None);
                    string fromDate = dfrom.ToShortDateString().Replace("-", "/");
                   // DateTime dTo = dfrom.AddDays(6);
                   // string toDate = dTo.ToShortDateString().Replace("-", "/");

                    string date = DateTime.Now.ToShortDateString();
                    int days = (dfrom - DateTime.Now).Days;

                    for (var date1 = dfrom; dfrom <= DateTime.Now; dfrom = dfrom.AddDays(6))
                    {
                       // DateTime dfrom = DateTime.ParseExact(lisEmpSync[lisEmpSync.Count - 1].SyncedOn.ToString(), "dd-MM-yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None);
                        string fromDate1 = date1.ToShortDateString().Replace("-", "/");
                        DateTime dTo1 = dfrom.AddDays(6);
                        string toDate1 = dTo1.ToShortDateString().Replace("-", "/");
                        var emp = new NewEmployeeListRequest { APIKey = "32a75a44-c927-42fb-b8ec-8d0898880d90", CorpURL = "https://motivitylabs.com", DateFrom = fromDate, DateTo = toDate1 };
                        var Response = await client.PostAsJsonAsync("https://motivity.eazework.com/EazeAPI/Employee.svc/NewEmployeeList", emp);
                        string empobj = await Response.Content.ReadAsStringAsync();
                        NewEmployeeListResultResponse response = JsonConvert.DeserializeObject<NewEmployeeListResultResponse>(empobj);
                        if (response.NewEmployeeListResult.ErrorCode == 0)
                        {
                            if (response != null)
                            {
                                if (response.NewEmployeeListResult.Data.Count > 0)
                                {
                                    int dbchangesNewEmpList = 0;
                                    foreach (Data empdata in response.NewEmployeeListResult.Data)
                                    {
                                        List<EmployeeDetail> empList = _dbContext.EmployeeDetails.ToList();
                                        EmployeeDetail ed = empList.Find(s => s.EmpCode == empdata.EmpCode);
                                        if (ed == null)
                                        {
                                            EmployeeDetail empl = new EmployeeDetail();
                                            empl.EmailId = empdata.EmailID;
                                            empl.FirstName = empdata.FirstName;
                                            empl.LastName = empdata.LastName;
                                            empl.Address = empdata.Address;
                                            empl.Address1 = empdata.Address1;
                                            empl.City = empdata.City;
                                            empl.Company = empdata.Company;
                                            empl.Country = empdata.Country;
                                            if (empdata.DOB != null)
                                            {
                                                empl.Dob = DateTime.ParseExact(empdata.DOB, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None);
                                            }
                                            if (empdata.DOJ != null)
                                            {
                                                empl.Doj = DateTime.ParseExact(empdata.DOJ, "dd/MM/yyyy", null);
                                            }
                                            empl.DataChange = empdata.DataChange;
                                            empl.Department = empdata.Department;
                                            empl.Designation = empdata.Designation;
                                            empl.Division = empdata.Division;
                                            empl.EmpType = empdata.EmpType;
                                            empl.EmploymentStatus = empdata.EmploymentStatus;
                                            empl.FunctionalManager = empdata.FunctionalManager;
                                            empl.FunctionalManagerCode = empdata.FunctionalManagerCode;
                                            empl.Gender = empdata.Gender;
                                            empl.Lwd = empdata.LWD;
                                            empl.Level = empdata.Level;
                                            empl.Manager = empdata.Manager;
                                            empl.ManagerCode = empdata.ManagerCode;
                                            empl.MiddleName = empdata.MiddleName;
                                            empl.MobileNo = empdata.MobileNo;
                                            empl.Name = empdata.Name;
                                            empl.NewTranId = empdata.NewTranID;
                                            empl.OfficeLocation = empdata.OfficeLocation;
                                            empl.OfficeLocationCity = empdata.OfficeLocationCity;
                                            if (empdata.OnDate != null)
                                            {
                                                empl.OnDate = DateTime.ParseExact(empdata.OnDate, "dd/MM/yyyy", null);
                                            }
                                            empl.Pin = empdata.Pin;
                                            if (empdata.SepSubmissionDate != null)
                                            {
                                                empl.SepSubmissionDate = DateTime.ParseExact(empdata.SepSubmissionDate, "dd/MM/yyyy", null);
                                            }
                                            empl.State = empdata.State;
                                            empl.Status = empdata.Status;
                                            empl.WorkLocation = empdata.WorkLocation;
                                            empl.WorkLocationCity = empdata.WorkLocationCity;
                                            empl.WorkNature = empdata.WorkNature;
                                            empl.EmpCode = empdata.EmpCode;
                                            if (empdata.CustomAdditional != null)
                                            {
                                                var CustomAdditionalJson = JsonConvert.SerializeObject(empdata.CustomAdditional);
                                                empl.CustomAdditional = CustomAdditionalJson;
                                            }
                                            if (empdata.CustomOfficial != null)
                                            {
                                                var CustomOfficialJson = JsonConvert.SerializeObject(empdata.CustomOfficial);
                                                empl.CustomOfficial = CustomOfficialJson;
                                            }
                                            if (empdata.CustomPersonal != null)
                                            {
                                                var CustomPersonalJson = JsonConvert.SerializeObject(empdata.CustomPersonal);
                                                empl.CustomPersonal = CustomPersonalJson;
                                            }
                                            try
                                            {
                                                _dbContext.EmployeeDetails.Add(empl);
                                                dbchangesNewEmpList = _dbContext.SaveChanges();
                                            }
                                            catch
                                            {
                                                dbchangesNewEmpList = 0;
                                            }

                                        }
                                        else
                                        {
                                        }
                                    }
                                    //if (dbchangesNewEmpList > 0)
                                    //{
                                    //    EmployeeSyncing es = new EmployeeSyncing();
                                    //    es.SyncedOn = DateTime.ParseExact(DateTime.Now.ToString(), "dd-MM-yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None);
                                    //    es.SyncedBy = "Test";
                                    //    _dbContext.EmployeeSyncing.Add(es);
                                    //    _dbContext.SaveChanges();
                                    //}


                                }
                            }


                        }


                    }

                }



            }
            //if (ModelState.IsValid)
            //{
                using (var client = new HttpClient())
                {
                    List<EmployeeSyncing> lisEmpSync = _dbContext.EmployeeSyncing.ToList();
                    if (lisEmpSync.Count > 0)
                    {
                        DateTime dfrom = DateTime.ParseExact(lisEmpSync[lisEmpSync.Count - 1].SyncedOn.ToString(), "dd-MM-yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None);
                    //DateTime dfrom = DateTime.ParseExact(DateTime.Now.ToString(), "dd-MM-yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None);
                    //string fromDate = dfrom.ToShortDateString().Replace("-", "/");
                    //DateTime dTo = dfrom.AddDays(6);
                    //string toDate = dTo.ToShortDateString().Replace("-", "/");
                    string date = DateTime.Now.ToShortDateString();

                    for (var date1 = dfrom; dfrom <= DateTime.Now; dfrom = dfrom.AddDays(6))
                    {
                        string fromDate1 = dfrom.ToShortDateString().Replace("-", "/");
                        DateTime dTo = dfrom.AddDays(6);
                        string toDate = dTo.ToShortDateString().Replace("-", "/");

                        // var emp = new NewEmployeeListRequest { APIKey = "32a75a44-c927-42fb-b8ec-8d0898880d90", CorpURL = "https://motivitylabs.com", DateFrom = "28/10/2019", DateTo = "03/11/2019" };
                        var emp = new NewEmployeeListRequest { APIKey = "32a75a44-c927-42fb-b8ec-8d0898880d90", CorpURL = "https://motivitylabs.com", DateFrom = fromDate1, DateTo = toDate };
                        var Response = await client.PostAsJsonAsync("https://motivity.eazework.com/EazeAPI/Employee.svc/UpdatedEmployeeList", emp);
                        string empobj = await Response.Content.ReadAsStringAsync();
                        UpdatedEmployeeListResultResponse response = JsonConvert.DeserializeObject<UpdatedEmployeeListResultResponse>(empobj);
                        if (response.UpdatedEmployeeListResult.ErrorCode == 0)
                        {
                            if (response != null)
                            {
                                if (response.UpdatedEmployeeListResult.Data.Count > 0)
                                {
                                    int dbchangesForUpdateEmp = 0;
                                    foreach (Data empdata in response.UpdatedEmployeeListResult.Data)
                                    {
                                        List<EmployeeDetail> empList = _dbContext.EmployeeDetails.ToList();
                                        EmployeeDetail ed = empList.Find(s => s.EmpCode == empdata.EmpCode);
                                        if (ed != null)
                                        {

                                            //EmployeeDetail empl = new EmployeeDetail();
                                            if (empdata.EmailID != null)
                                            {
                                                ed.EmailId = empdata.EmailID;
                                            }
                                            if (empdata.FirstName != null)
                                            {
                                                ed.FirstName = empdata.FirstName;
                                            }
                                            if (empdata.LastName != null)
                                            {
                                                ed.LastName = empdata.LastName;
                                            }
                                            if (empdata.Address != null)
                                            {
                                                ed.Address = empdata.Address;
                                            }
                                            if (empdata.Address1 != null)
                                            {
                                                ed.Address1 = empdata.Address1;
                                            }
                                            if (empdata.City != null)
                                            {
                                                ed.City = empdata.City;
                                            }
                                            if (empdata.Company != null)
                                            {
                                                ed.Company = empdata.Company;
                                            }
                                            if (empdata.Country != null)
                                            {
                                                ed.Country = empdata.Country;
                                            }
                                            if (!string.IsNullOrEmpty(empdata.DOB))
                                            {
                                                ed.Dob = DateTime.ParseExact(empdata.DOB, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None);
                                            }
                                            if (empdata.DOJ != null)
                                            {
                                                ed.Doj = DateTime.ParseExact(empdata.DOJ, "dd/MM/yyyy", null);
                                            }
                                            if (empdata.DataChange != null)
                                            {
                                                ed.DataChange = empdata.DataChange;
                                            }
                                            if (empdata.Department != null)
                                            {
                                                ed.Department = empdata.Department;
                                            }
                                            if (empdata.Designation != null)
                                            {
                                                ed.Designation = empdata.Designation;
                                            }
                                            if (empdata.Division != null)
                                            {
                                                ed.Division = empdata.Division;
                                            }
                                            if (empdata.EmpType != null)
                                            {
                                                ed.EmpType = empdata.EmpType;
                                            }
                                            if (empdata.EmploymentStatus != null)
                                            {
                                                ed.EmploymentStatus = empdata.EmploymentStatus;
                                            }
                                            if (empdata.FunctionalManager != null)
                                            {
                                                ed.FunctionalManager = empdata.FunctionalManager;
                                            }
                                            if (empdata.FunctionalManagerCode != null)
                                            {
                                                ed.FunctionalManagerCode = empdata.FunctionalManagerCode;
                                            }
                                            if (empdata.Gender != null)
                                            {
                                                ed.Gender = empdata.Gender;
                                            }
                                            if (empdata.LWD != null)
                                            {
                                                ed.Lwd = empdata.LWD;
                                            }
                                            if (empdata.Level != null)
                                            {
                                                ed.Level = empdata.Level;
                                            }
                                            if (empdata.Manager != null)
                                            {
                                                ed.Manager = empdata.Manager;
                                            }
                                            if (empdata.ManagerCode != null)
                                            {
                                                ed.ManagerCode = empdata.ManagerCode;
                                            }
                                            if (empdata.MiddleName != null)
                                            {
                                                ed.MiddleName = empdata.MiddleName;
                                            }
                                            if (empdata.MobileNo != null)
                                            {
                                                ed.MobileNo = empdata.MobileNo;
                                            }
                                            if (empdata.Name != null)
                                            {
                                                ed.Name = empdata.Name;
                                            }
                                            if (empdata.NewTranID != null)
                                            {
                                                ed.NewTranId = empdata.NewTranID;
                                            }
                                            if (empdata.OfficeLocation != null)
                                            {
                                                ed.OfficeLocation = empdata.OfficeLocation;
                                            }
                                            if (empdata.OfficeLocationCity != null)
                                            {
                                                ed.OfficeLocationCity = empdata.OfficeLocationCity;
                                            }
                                            if (empdata.OnDate != null)
                                            {
                                                ed.OnDate = DateTime.ParseExact(empdata.OnDate, "dd/MM/yyyy", null);
                                            }
                                            if (empdata.Pin != null)
                                            {
                                                ed.Pin = empdata.Pin;
                                            }
                                            if (empdata.SepSubmissionDate != null)
                                            {
                                                ed.SepSubmissionDate = DateTime.ParseExact(empdata.SepSubmissionDate, "dd/MM/yyyy", null);
                                            }
                                            if (empdata.State != null)
                                            {
                                                ed.State = empdata.State;
                                            }
                                            if (empdata.Status != null)
                                            {
                                                ed.Status = empdata.Status;
                                            }
                                            if (empdata.WorkLocation != null)
                                            {
                                                ed.WorkLocation = empdata.WorkLocation;
                                            }
                                            if (empdata.WorkLocationCity != null)
                                            {
                                                ed.WorkLocationCity = empdata.WorkLocationCity;
                                            }
                                            if (empdata.WorkNature != null)
                                            {
                                                ed.WorkNature = empdata.WorkNature;
                                            }
                                            if (empdata.EmpCode != null)
                                            {
                                                ed.EmpCode = empdata.EmpCode;
                                            }
                                            if (empdata.CustomAdditional != null && empdata.CustomAdditional.Count > 0)
                                            {
                                                List<CustomAdditionals> customAdditionalAPi = empdata.CustomAdditional;
                                                List<CustomAdditional> customAdditionalLocal = JsonConvert.DeserializeObject<List<CustomAdditional>>(ed.CustomAdditional);
                                                List<CustomAdditional> customAdditionalTepm = new List<CustomAdditional>();
                                                foreach (CustomAdditional cd in customAdditionalLocal)
                                                {
                                                    CustomAdditional cd3 = new CustomAdditional();
                                                    CustomAdditionals cadl = customAdditionalAPi.Find(s => s.FieldName == cd.FieldName);
                                                    if (cadl != null)
                                                    {
                                                        cd3.FieldName = cadl.FieldName;
                                                        cd3.FieldValue = cadl.FieldValue;
                                                    }
                                                    else
                                                    {
                                                        cd3.FieldName = cd.FieldName;
                                                        cd3.FieldValue = cd.FieldValue;
                                                    }
                                                    customAdditionalTepm.Add(cd3);

                                                }
                                                var CustomAdditionalJson = JsonConvert.SerializeObject(customAdditionalTepm);
                                                ed.CustomAdditional = CustomAdditionalJson;

                                            }
                                            if (empdata.CustomOfficial != null && empdata.CustomOfficial.Count > 0)
                                            {
                                                List<CustomOfficials> customOfficialAPI = empdata.CustomOfficial;
                                                List<CustomOfficial> customOfficialLocal = JsonConvert.DeserializeObject<List<CustomOfficial>>(ed.CustomOfficial);
                                                List<CustomOfficial> customOfficialTepm = new List<CustomOfficial>();
                                                foreach (CustomOfficial cd in customOfficialLocal)
                                                {
                                                    CustomOfficial cd3 = new CustomOfficial();
                                                    CustomOfficials cadl = customOfficialAPI.Find(s => s.FieldName == cd.FieldName);
                                                    if (cadl != null)
                                                    {
                                                        cd3.FieldName = cadl.FieldName;
                                                        cd3.FieldValue = cadl.FieldValue;
                                                    }
                                                    else
                                                    {
                                                        cd3.FieldName = cd.FieldName;
                                                        cd3.FieldValue = cd.FieldValue;
                                                    }
                                                    customOfficialTepm.Add(cd3);

                                                }
                                                var CustomOfficialJson = JsonConvert.SerializeObject(customOfficialTepm);
                                                ed.CustomAdditional = CustomOfficialJson;
                                            }
                                            if (empdata.CustomPersonal != null && empdata.CustomPersonal.Count > 0)
                                            {
                                                List<CustomPersonals> customPersonalAPI = empdata.CustomPersonal;
                                                List<CustomPersonals> customPersonalLocal = JsonConvert.DeserializeObject<List<CustomPersonals>>(ed.CustomPersonal);

                                                List<CustomPersonals> customPersonalTepm = new List<CustomPersonals>();
                                                foreach (CustomPersonals cd in customPersonalLocal)
                                                {
                                                    CustomPersonals cd3 = new CustomPersonals();
                                                    CustomPersonals cadl = customPersonalAPI.Find(s => s.FieldName == cd.FieldName);
                                                    if (cadl != null)
                                                    {
                                                        cd3.FieldName = cadl.FieldName;
                                                        cd3.FieldValue = cadl.FieldValue;
                                                    }
                                                    else
                                                    {
                                                        cd3.FieldName = cd.FieldName;
                                                        cd3.FieldValue = cd.FieldValue;
                                                    }
                                                    customPersonalTepm.Add(cd3);

                                                }
                                                var CustomPersonalJson = JsonConvert.SerializeObject(customPersonalTepm);
                                                ed.CustomPersonal = CustomPersonalJson;

                                            }
                                            try
                                            {
                                                _dbContext.EmployeeDetails.Update(ed);
                                                dbchangesForUpdateEmp = _dbContext.SaveChanges();
                                            }
                                            catch
                                            {
                                                dbchangesForUpdateEmp = 0;
                                            }

                                        }
                                        else
                                        {
                                        }

                                    }
                                    //if (dbchangesForUpdateEmp > 0)
                                    //{

                                    //    EmployeeSyncing es = new EmployeeSyncing();
                                    //    es.SyncedOn = DateTime.ParseExact(DateTime.Now.ToString(), "dd-MM-yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None);
                                    //    es.SyncedBy = "Test";
                                    //    _dbContext.EmployeeSyncing.Add(es);
                                    //    _dbContext.SaveChanges();
                                    //}


                                }


                            }


                        }
                    }
                    }
                    //TempData["Message"] = "Employee data is synced from Eazework portal.";
               // return RedirectToAction(actionName: "Index", controllerName: "Employee");
                }
            //    return RedirectToAction(actionName: "Index", controllerName: "Employee");
            //}
            EmployeeSyncing es = new EmployeeSyncing();
            es.SyncedOn = DateTime.ParseExact(DateTime.Now.ToString(), "dd-MM-yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None);
            es.SyncedBy = "Test";
            _dbContext.EmployeeSyncing.Add(es);
            _dbContext.SaveChanges();

            TempData["Message"] = "Employee data is synced from Eazework portal.";
            return RedirectToAction(actionName: "Index", controllerName: "Employee");
        }

    }
    
}
