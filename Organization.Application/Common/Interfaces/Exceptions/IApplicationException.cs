using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Organization.Application.Common.Interfaces.Exceptions
{
    public  interface IApplicationException
    {
        public HttpStatusCode StatusCode { get; }
        public string ErrorMessage { get; }  
    }
}
