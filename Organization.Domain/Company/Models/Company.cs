using Organization.Domain.Common.Models;
using Organization.Domain.Common.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organization.Domain.Company.Models
{
    [TableName("tblCompanies")]
    public sealed class Company : IDbEntity
    {
        [ColumnName("Id")]
        public string? Id { get; set; } = ShortGuid.NewGuid();
        [DistinguishingUniqueKey]
        [ColumnName("Name")]
        public string? Name { get; set; }
        [ColumnName("Country")]
        public string? Country { get; set; }
        [ColumnName("Address")]
        public string? Address { get; set; }
        [ColumnName("IsDeleted")]
        public bool IsDeleted { get; set; }
        // [Navigation(typeof(child table/referenced table), "foreign key")]
        [Navigation(typeof(Employee.Models.Employee), "CompanyId")]
        public IEnumerable<Employee.Models.Employee> Employees { get; set; } = new List<Employee.Models.Employee>();
    }

}
