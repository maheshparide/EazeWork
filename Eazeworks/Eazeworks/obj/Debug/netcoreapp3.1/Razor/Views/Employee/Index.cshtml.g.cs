#pragma checksum "C:\Eazeworks\Eazeworks\Eazeworks\Views\Employee\Index.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "32cdf602d72f4f04a10c59b7589e3a33d7a9a12d"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Employee_Index), @"mvc.1.0.view", @"/Views/Employee/Index.cshtml")]
namespace AspNetCore
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#nullable restore
#line 1 "C:\Eazeworks\Eazeworks\Eazeworks\Views\_ViewImports.cshtml"
using Eazeworks;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "C:\Eazeworks\Eazeworks\Eazeworks\Views\_ViewImports.cshtml"
using Eazeworks.Models;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"32cdf602d72f4f04a10c59b7589e3a33d7a9a12d", @"/Views/Employee/Index.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"56d5028aa6edd4ab3b93751cedede594bd7749f1", @"/Views/_ViewImports.cshtml")]
    public class Views_Employee_Index : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-action", "SyncData", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_1 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-controller", "Employee", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_2 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("style", new global::Microsoft.AspNetCore.Html.HtmlString("margin-left:5px;"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_3 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-action", "EmployeesList", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        #line hidden
        #pragma warning disable 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperExecutionContext __tagHelperExecutionContext;
        #pragma warning restore 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner __tagHelperRunner = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner();
        #pragma warning disable 0169
        private string __tagHelperStringValueBuffer;
        #pragma warning restore 0169
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __backed__tagHelperScopeManager = null;
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __tagHelperScopeManager
        {
            get
            {
                if (__backed__tagHelperScopeManager == null)
                {
                    __backed__tagHelperScopeManager = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager(StartTagHelperWritingScope, EndTagHelperWritingScope);
                }
                return __backed__tagHelperScopeManager;
            }
        }
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper;
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.FormTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper;
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.RenderAtEndOfFormTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("<div style=\"align-content:center;font-size:18px\">\r\n    <label style=\"align-content:center;font-size:18px\">Report was last synced from Eazework on.  </label><b style=\"align-content:center;font-size:18px;margin-left:2px\">");
#nullable restore
#line 7 "C:\Eazeworks\Eazeworks\Eazeworks\Views\Employee\Index.cshtml"
                                                                                                                                                                   Write(ViewBag.lastSyncedDate);

#line default
#line hidden
#nullable disable
            WriteLiteral("</b>\r\n</div>\r\n<hr />\r\n<div style=\"align-content:center;font-size:18px\">\r\n\r\n\r\n    <label>Click on Sync to update records from Eazework portal.</label>");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("a", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "32cdf602d72f4f04a10c59b7589e3a33d7a9a12d5325", async() => {
                WriteLiteral("<button style=\"border:solid;border-width:thin\">Sync</button>");
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.Action = (string)__tagHelperAttribute_0.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_0);
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.Controller = (string)__tagHelperAttribute_1.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_1);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_2);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("\r\n</div>\r\n<hr />\r\n<label><b>Select fields and generate report :</b></label>\r\n<div>\r\n    ");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("form", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "32cdf602d72f4f04a10c59b7589e3a33d7a9a12d6919", async() => {
                WriteLiteral("\r\n");
                WriteLiteral(@"        <div>
            <span>
                <input id=""EmployeeCode"" type=""checkbox"" name=EmployeeCode value=EmployeeCode checked=""checked"" />
                <label for=""EmployeeCode"">Employee Code</label>
            </span>
            <span class=""col-sm-2"" style=""margin-left:50px"">
                <input id=""EmployeeName"" type=""checkbox"" name=EmployeeName value=EmployeeName checked=""checked"" />
                <label for=""EmployeeName"">Employee Name</label>
            </span>
            <span class=""col-sm-4"" style=""margin-left:50px"">
                <input id=""ProjectName"" type=""checkbox"" name=ProjectName value=ProjectName checked=""checked"" />
                <label for=""ProjectName"">Project Name</label>
            </span>
            <span class=""col-sm-6"" style=""margin-left:50px"">
                <input id=""Designation"" type=""checkbox"" name=Designation value=Designation checked=""checked"" />
                <label for=""Designation"">Designation</label>
            </span>
      ");
                WriteLiteral(@"      <span class=""col-sm-8"" style=""margin-left:50px"">
                <input id=""Department"" type=""checkbox"" name=Department value=Department checked=""checked"" />
                <label for=""Department"">Department</label>
            </span>
        </div>
        <div>
            <span>
                <input id=""SubDepartment"" type=""checkbox"" name=SubDepartment value=SubDepartment checked=""checked"" />
                <label for=""SubDepartment"">Sub Department</label>
            </span>
            <span class=""col-sm-2"" style=""margin-left:45px"">
                <input id=""WorkLocation"" type=""checkbox"" name=WorkLocation value=WorkLocation checked=""checked"" />
                <label for=""WorkLocation"">Work Location</label>
            </span>
            <span class=""col-sm-4"" style=""margin-left:65px"">
                <input id=""Level"" type=""checkbox"" name=Level value=Level checked=""checked"" />
                <label for=""Level"">Level</label>
            </span>
            <span class=""co");
                WriteLiteral(@"l-sm-7"" style=""margin-left:110px"">
                <input id=""Email"" type=""checkbox"" name=Email value=Email checked=""checked"" />
                <label for=""Email"">Email</label>
            </span>
            <span class=""col-sm-9"" style=""margin-left:95px"">
                <input id=""ManagerEmail"" type=""checkbox"" name=ManagerEmail value=ManagerEmail checked=""checked"" />
                <label for=""ManagerEmail"">Manager Email</label>
            </span>
        </div>
        <div>
            <span>
                <input id=""CountryCode"" type=""checkbox"" name=CountryCode value=CountryCode checked=""checked"" />
                <label for=""CountryCode"">Country Code</label>
            </span>
            <span class=""col-sm-2"" style=""margin-left:65px"">
                <input id=""Mobile"" type=""checkbox"" name=Mobile value=Mobile checked=""checked"" />
                <label for=""Mobile"">Mobile</label>
            </span>
            <span class=""col-sm-4"" style=""margin-left:116px"">
              ");
                WriteLiteral(@"  <input id=""DateOfJoining"" type=""checkbox"" name=DateOfJoining value=DateOfJoining checked=""checked"" />
                <label for=""DateOfJoining"">Date Of Joining</label>
            </span>
            <span class=""col-sm-6"" style=""margin-left:37px"">
                <input id=""Gender"" type=""checkbox"" name=Gender value=Gender checked=""checked"" />
                <label for=""Gender"">Gender</label>

            </span>
            <span class=""col-sm-8"" style=""margin-left:83px"">
                <input id=""EmployeeType"" type=""checkbox"" name=EmployeeType value=EmployeeType checked=""checked"" />
                <label for=""EmployeeType"">Employee Type</label>
            </span>
        </div>
        <div>
            <span >
                <input id=""EmploymentStatus"" type=""checkbox"" name=EmploymentStatus value=EmploymentStatus checked=""checked"" />
                <label for=""EmploymentStatus"">Employment Status</label>
            </span>
            <span class=""col-sm-2"" style=""margin-left:25p");
                WriteLiteral(@"x"">
                <input id=""DeskNo"" type=""checkbox"" name=DeskNo value=DeskNo checked=""checked"" />
                <label for=""DeskNo"">Desk No</label>
            </span>
            <span class=""col-sm-4"" style=""margin-left:107px"">

                <input id=""PrevExp"" type=""checkbox"" name=PrevExp value=PrevExp checked=""checked"" />
                <label for=""PrevExp"">Prev Exp</label>
            </span>
            <span class=""col-sm-6"" style=""margin-left:87px"">
                <input id=""CurrentExperience"" type=""checkbox"" name=CurrentExperience value=CurrentExperience checked=""checked"" />
                <label for=""CurrentExperience"">Current Experience</label>
            </span>
            <span class=""col-sm-8"" style=""margin-left:1px"">
                <input id=""TotalExperience"" type=""checkbox"" name=TotalExperience value=TotalExperience checked=""checked"" />
                <label for=""TotalExperience"">Total Experience</label>
            </span>


        </div>
        <div>
   ");
                WriteLiteral(@"         <span>
                <input id=""Billing"" type=""checkbox"" name=Billing value=Billing checked=""checked"" />
                <label for=""Billing"">Billing</label>
            </span>
            <span class=""col-sm-2"" style=""margin-left:118px"">
                <input id=""CoreSkills"" type=""checkbox"" name=CoreSkills value=CoreSkills checked=""checked"" />
                <label for=""CoreSkills"">Core Skills</label>
            </span>
            <span class=""col-sm-4"" style=""margin-left:95px"">
                <input id=""MinorSkills"" type=""checkbox"" name=MinorSkills value=MinorSkills checked=""checked"" />
                <label for=""MinorSkills"">Minor Skills</label>
            </span>
            <span class=""col-sm-6"" style=""margin-left:65px"">
                <input id=""Certifications"" type=""checkbox"" name=Certifications value=Certifications checked=""checked"" />
                <label for=""Certifications"">Certifications</label>
            </span>
            <span class=""col-sm-8""style=""mar");
                WriteLiteral("gin-left:43px\">\r\n                <input id=\"Customer\" type=\"checkbox\" name=Customer value=Customer checked=\"checked\" />\r\n                <label for=\"Customer\">Customer</label>\r\n            </span>\r\n        </div>\r\n\r\n\r\n\r\n\r\n");
                WriteLiteral("        <div style=\"align-content:center;text-align:center;margin-right:50px\">\r\n            <input type=\"submit\" style=\"border:solid;border-width:thin\" value=\"Generate Report\" />\r\n        </div>\r\n    ");
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.FormTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.RenderAtEndOfFormTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper.Action = (string)__tagHelperAttribute_3.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_3);
            __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper.Controller = (string)__tagHelperAttribute_1.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_1);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("\r\n</div>\r\n");
#nullable restore
#line 145 "C:\Eazeworks\Eazeworks\Eazeworks\Views\Employee\Index.cshtml"
 if (TempData["Message"] != null)
{

#line default
#line hidden
#nullable disable
            WriteLiteral("    <script type=\"text/javascript\">\r\n            window.onload = function () {\r\n                alert(\"");
#nullable restore
#line 149 "C:\Eazeworks\Eazeworks\Eazeworks\Views\Employee\Index.cshtml"
                  Write(TempData["Message"]);

#line default
#line hidden
#nullable disable
            WriteLiteral("\");\r\n            };\r\n    </script>\r\n");
#nullable restore
#line 152 "C:\Eazeworks\Eazeworks\Eazeworks\Views\Employee\Index.cshtml"
}

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n");
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<dynamic> Html { get; private set; }
    }
}
#pragma warning restore 1591
