using Organization.Application.Common.Interfaces.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Organization.Application.Common.Exceptions
{
    public class DuplicateException : Exception, IApplicationException
    {
        public DuplicateException(string errorMessage) : base(errorMessage) { } 
        public HttpStatusCode StatusCode => HttpStatusCode.Conflict;

        public string ErrorMessage => Message;
    }
}
