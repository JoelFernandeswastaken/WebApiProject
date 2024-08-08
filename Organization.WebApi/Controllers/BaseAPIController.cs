using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Net;

namespace Organization.Presentation.Api.Controllers
{
    public class BaseAPIController : ControllerBase
    {
        protected IActionResult Problem(List<Error> errors)
        {
            if(errors.Count < 1)
            {
                return Problem();
            }
            if(errors.All(p => p.Type == ErrorType.Validation))
            {
                return ValidationProblem(errors);
            }
            return Problem(errors[0]);
        }
        protected IActionResult Problem(Error error)
        {
            var statusCode = error.Type switch
            {
                ErrorType.Conflict => Convert.ToInt32(HttpStatusCode.Conflict),
                ErrorType.NotFound => Convert.ToInt32(HttpStatusCode.NotFound),
                ErrorType.Validation => Convert.ToInt32(HttpStatusCode.BadRequest),
                _ => Convert.ToInt32(HttpStatusCode.InternalServerError)
            };
            return Problem(statusCode: statusCode, title: error.Description);
        }
        protected IActionResult ValidationProblem(List<Error> errors)
        {
            var dictionary = new ModelStateDictionary();    
            foreach(var error in errors)
            {
                dictionary.AddModelError(error.Code, error.Description);
            }
            return ValidationProblem(dictionary);
        }
    }
}
