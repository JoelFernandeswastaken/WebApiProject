using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Organization.Application.Common.Interfaces.Exceptions;
using System.Net;

namespace Organization.Presentation.Api.Controllers
{
    [Route("/error")]
    public sealed class ErrorController : ControllerBase
    {
        public IActionResult Error()
        {
            var exception = HttpContext.Features.Get<IExceptionHandlerFeature>().Error;
            var (statusCode, message) = exception switch
            {
                IApplicationException appException => (Convert.ToInt32(appException.StatusCode), appException.ErrorMessage),
                _ => (StatusCodes.Status500InternalServerError, exception.Message) // handles default condition
            };
            return Problem(statusCode: statusCode, title: message );

            //var exception = HttpContext.Features.Get<IExceptionHandlerFeature>().Error;
            //return Problem(statusCode: 500, title: exception.Message);
            // return Problem(statusCode: 500);
        }
    }
}
