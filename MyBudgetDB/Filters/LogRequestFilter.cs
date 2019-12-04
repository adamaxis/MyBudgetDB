using Microsoft.AspNetCore.Mvc.Filters;
using MyBudgetDB.Models.FilterModels;
using MyBudgetDB.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyBudgetDB.Filters
{
    public class LogRequestAsyncAttribute : Attribute, IAsyncResourceFilter
    {
        public bool IsEnabled { get; set; }

        public async Task OnResourceExecutionAsync(ResourceExecutingContext context, ResourceExecutionDelegate next)
        {
            if (IsEnabled)
            {
                Guid requestId = Guid.NewGuid();
                context.HttpContext.Response.Headers.Add("requestId", requestId.ToString());
                var executedContext = await next();

                var service = (BudgetService)context.HttpContext.RequestServices.GetService(typeof(BudgetService));
                LogRequestModel lrm = new LogRequestModel
                {
                    RequestId = requestId,
                    Request = context.HttpContext.Request.Path.ToString(), // request path? I don't know much about HTTP headers
                    Response = context.HttpContext.Response.StatusCode.ToString() // response code
                };
                service.CreateAccessLog(lrm);
            }
        }
    }
}
