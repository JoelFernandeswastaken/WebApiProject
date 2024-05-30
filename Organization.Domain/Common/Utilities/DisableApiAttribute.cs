using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.Controllers;
//using Microsoft.Extensions.Configuration;
//using Microsoft.OpenApi.Models;
//using Swashbuckle.AspNetCore.SwaggerGen;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Web.Mvc;

namespace Organization.Domain.Common.Utilities
{
    public class DisableApiAttribute : ActionFilterAttribute, IDocumentFilter
    {
        private IConfigurationRoot _configuration;
        // removes endpoints from swaggerDoc
        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            var builder = new ConfigurationBuilder().SetBasePath(AppDomain.CurrentDomain.BaseDirectory);
            _configuration = builder.Build();
            var apiToDisable = _configuration.GetSection("DisableAPIs").Get<List<string>>();
            if(apiToDisable.Count > 0)
                apiToDisable.Select(a => "/" + a.Trim()).Where(b => swaggerDoc.Paths.ContainsKey(b)).ToList().ForEach(x => swaggerDoc.Paths.Remove(x));
        }
        // return forbidden when endpoint hit
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            //var builder = new ConfigurationBuilder().SetBasePath(AppDomain.CurrentDomain.BaseDirectory);
            //_configuration = builder.Build();
            //var apiToDisable = _configuration.GetSection("DisableAPIs").Get<List<string>>();
            //filterContext.Result = new ForbidResult();
            var serviceProvider = context.HttpContext.RequestServices;
            var configuration = serviceProvider.GetRequiredService<IConfiguration>();

            var controllerActionDescriptor = context.ActionDescriptor as ControllerActionDescriptor;
            if (controllerActionDescriptor != null)
            {
                var disabledApis = configuration.GetSection("DisableAPIS")
                           .GetChildren()
                           .Select(x => x.Value)
                           .ToList();

                if (disabledApis != null && disabledApis.Count > 0 && disabledApis.Contains(context.ActionDescriptor.AttributeRouteInfo.Template))
                {
                    context.Result = new StatusCodeResult(403); // Forbidden
                    return;
                }
            }
            base.OnActionExecuting(context);
        }
    }
}
