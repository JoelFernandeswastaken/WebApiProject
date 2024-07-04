using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organization.Application.Common.DTO.Request
{
    public class EmployeeRequest
    {
        public int age { get; set; }
        public string name { get; set; }
        public string position { get; set; }
        public string companyID { get; set; }
        public int salary { get; set; }
    }
}
