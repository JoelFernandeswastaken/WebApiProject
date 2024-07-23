using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organization.Application.Common.DTO.Request
{
    public class EmployeeRequest
    {
        public int Age { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Position { get; set; } = string.Empty;
        public string CompanyID { get; set; } = string.Empty;
        public int Salary { get; set; }
    }
}
