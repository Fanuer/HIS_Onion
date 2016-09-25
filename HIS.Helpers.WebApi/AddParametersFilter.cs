using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNetCore.Mvc.Filters;
using Swashbuckle.Swagger.Model;
using Swashbuckle.SwaggerGen.Generator;

namespace HIS.Helpers.WebApi
{
    public class AddParametersFilter : IOperationFilter
    {
        public void Apply(Operation operation, OperationFilterContext context)
        {
            var hasAuthorizeAttr =
                context.ApiDescription.ActionDescriptor.FilterDescriptors.Select(x => x.Filter)
                    .Any(x => x is IAuthorizationFilter);
            var ad = context.ApiDescription.ActionDescriptor;
            var hasAllowAnonymous = false;

            if (hasAuthorizeAttr && !hasAllowAnonymous)
            {
                if (operation.Parameters == null)
                {
                    operation.Parameters = new List<IParameter>();
                }
                operation.Parameters.Add(new NonBodyParameter()
                {
                    Description =
                        "Authorization token. Used for applying content access restrictions. Use one of the OAuth2 grants to auto-populate this value.",
                    In = "header",
                    Name = "Authorization",
                    Required = true,
                    Type = "string",
                    Default = "bearer "
                });
            }
        }
    }
}