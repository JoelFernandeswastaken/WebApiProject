using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organization.Infrastructure.Utilities
{
    public sealed class LoggerGuid
    {
        public string GenerateCorrelationID()
        {
            string guid = Guid.NewGuid().ToString();
            return guid;
        }
    }
}
