using Organization.Domain.Common.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organization.Domain.Employee
{
    public sealed class EmployeeQueryParameters : QueryParameters
    {
        public string EmployeeName { get; set; }    
    }
}
