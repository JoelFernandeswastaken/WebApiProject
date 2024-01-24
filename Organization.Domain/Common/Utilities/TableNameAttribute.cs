using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organization.Domain.Common.Utilities
{
    // attributes can be used or a class or property. in this case class.
    [AttributeUsage(AttributeTargets.Class)]
    public class TableNameAttribute : Attribute
    {
        public string NameValue { get;  }   
        public TableNameAttribute(string nameValue) 
        {
            this.NameValue = nameValue; 
        }
    }
}
