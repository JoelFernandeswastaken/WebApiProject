using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Organization.Infrastructure.Utilities
{
    public sealed class CustomGuid
    {
        public string GenerateCorrelationID()
        {
            string guid = Guid.NewGuid().ToString();
            return guid;
        }
        public string GenerateEntityGuid()
        {
            string guid = Guid.NewGuid().ToString().Replace("/", "_").Replace("+", "-").Substring(0, 22);
            return guid; 
        }
    }
}
