using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organization.Domain.Common.Utilities
{
    internal class ShortGuid
    {
        public static string NewGuid()
        {
            var guid = Guid.NewGuid();
            byte[] bytes = guid.ToByteArray();
            string base64 = Convert.ToBase64String(bytes).Replace("/", "_").Replace("+", "-").Substring(0,22);
            return base64;
        }
    }
}
