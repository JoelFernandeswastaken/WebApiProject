using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organization.Domain.Common.Utilities
{
    [AttributeUsage(AttributeTargets.Property)]
    public class NavigationAttribute : Attribute
    {
        public Type AssociatedType { get; set; }
        public string AssociatedProperty { get; set; }  
        public NavigationAttribute(Type associatedType, string associatedProperty)
        { 
            this.AssociatedType = associatedType;
            this.AssociatedProperty = associatedProperty;
        }
    }
}
