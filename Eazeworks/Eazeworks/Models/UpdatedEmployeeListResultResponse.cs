using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eazeworks.Models
{
    public class UpdatedEmployeeListResultResponse
    {
        public UpdatedEmployeeListResult UpdatedEmployeeListResult { get; set; }
    }
    public class UpdatedEmployeeListResult
    {
        public int ErrorCode { get; set; }
        public string ErrorMessage { get; set; }
        public string MessageType { get; set; }
        public List<Data> Data { get; set; }
    }
    
}
