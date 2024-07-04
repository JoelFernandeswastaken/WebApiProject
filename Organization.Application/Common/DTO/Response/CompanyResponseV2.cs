using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organization.Application.Common.DTO.Response
{
    public class CompanyResponseV2
    {
        public List<CompanyUserSide> Companies { get; set; }
        public CompanyResponseV2() { }
    }
    public class CompanyUserSide
    {
        public string Id { get; set; }
        public string? Name { get; set; }
        public string? Country { get; set; }
        public string? Address { get; set; }
    }

}
