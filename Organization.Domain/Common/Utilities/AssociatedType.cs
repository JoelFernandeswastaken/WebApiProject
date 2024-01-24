using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Organization.Domain.Common.Utilities
{
    public class AssociatedType
    {
        public Type Type { get;  }
        public PropertyInfo ForeignKeyProperty { get; }
        public AssociatedType(Type type, PropertyInfo foreignKey)
        {
            Type = type;
            ForeignKeyProperty = foreignKey;
        }
    }
}
