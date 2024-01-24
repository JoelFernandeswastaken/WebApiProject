using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organization.Domain.Common.Models
{
    public interface IDbEntity
    {
        public string Id { get; set; }  
    }
}
