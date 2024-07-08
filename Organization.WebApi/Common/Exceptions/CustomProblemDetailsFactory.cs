using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Organization.Presentation.Api.Common.Exceptions
{
    public class CustomProblemDetailsFactory : ProblemDetailsFactory
    {
        public override ProblemDetails CreateProblemDetails(HttpContext httpContext, int? statusCode = null, string? title = null, string? type = null, string? detail = null, string? instance = null)
        {
            var problemDetails = new ProblemDetails
            {
                Status = statusCode, //You could delete these original property and only leave the Extensions property.
                Title = title,
                Type = RFC7231Mapper.GetRFC7231Link(statusCode),
                Detail = detail,
                Instance = instance,
                Extensions =
                    {
                        {"TraceID", httpContext.TraceIdentifier}
                    }
            };
            return problemDetails;

        }

        public override ValidationProblemDetails CreateValidationProblemDetails(HttpContext httpContext, ModelStateDictionary modelStateDictionary, int? statusCode = null, string? title = null, string? type = null, string? detail = null, string? instance = null)
        {
            var validationproblemdetails = new ValidationProblemDetails
            {
                Status = statusCode,
            };
            return validationproblemdetails;
        }
    }
}
