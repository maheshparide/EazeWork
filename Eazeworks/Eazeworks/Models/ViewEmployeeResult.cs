using Eazeworks.Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eazeworks.Models
{
    public class ViewEmployeeResult
    {
        public int ErrorCode { get; set; }

        public string ErrorMessage { get; set; }
        public string MessageType { get; set; }
        public Employee Data { get; set; }

    }
}
