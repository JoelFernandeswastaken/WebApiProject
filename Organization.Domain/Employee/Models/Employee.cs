using Organization.Domain.Common.Models;
using Organization.Domain.Common.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organization.Domain.Employee.Models
{
    [TableName("tblEmployees")]
    public class Employee : IDbEntity
    {
        [ColumnName("Id")]
        public string? Id { get; set; } = ShortGuid.NewGuid();
        [ColumnName("Name")]
        public string? Name { get; set; }
        [ColumnName("CompanyId")]
        public string? CompanyId { get; set; }
        [ColumnName("Age")]
        public int Age { get; set; }
        [ColumnName("Position")]
        public string? Position { get; set; }
        [ColumnName("Salary")]
        public decimal Salary { get; set; }
        [ColumnName("CreatedOn")]
        public string CreatedOn { get; set; }
        [ColumnName("ModifiedOn")]
        public string ModifiedOn { get; set; }
    }
}
