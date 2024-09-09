using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organization.Application.Common.DTO.Response
{
    public sealed record class EmployeeResponseV2(string? Name, int Age, string? Position, int Salary, string? CreatedOn);
    public class EmployeeResponse
    {
        public string Name { get; set; } = string.Empty;
        public int Age { get; set; }
        public string Position { get; set; } = string.Empty;
        public int Salary { get; set; }
        public string CreatedOn { get; set; }
    }
}
