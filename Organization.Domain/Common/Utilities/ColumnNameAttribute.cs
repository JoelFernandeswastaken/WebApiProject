using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;

namespace Organization.Domain.Common.Utilities
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ColumnNameAttribute : Attribute
    {
        public string NameValue { get; set; }
        public ColumnNameAttribute(string nameValue) 
        { 
            this.NameValue = nameValue;
        }

    }
}
