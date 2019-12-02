using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using MyBudgetDB.Models.BudgetCommands;

namespace MyBudgetDB.Attributes
{
    public class AddLastModifiedHeader : ResultFilterAttribute
    {
        public override void OnResultExecuting(ResultExecutingContext context)
        {
            if (context.Result is OkObjectResult result
                && result.Value is UserBudgetDetails detail)
            {
                var lastModified = context.HttpContext.Request.GetTypedHeaders().IfModifiedSince;

                if (lastModified.HasValue && lastModified >= detail.LastModified)
                {
                    context.Result = new StatusCodeResult(304);
                }
            }
        }
    }
}
