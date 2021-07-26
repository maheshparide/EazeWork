using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eazeworks.Models
{
    public class NewEmployeeListRequest
    {
        public string APIKey { get; set; }
        public string CorpURL { get; set; }
        public string DateFrom { get; set; }
        public string DateTo { get; set; }
    }
}
