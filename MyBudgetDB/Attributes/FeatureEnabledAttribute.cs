using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace MyBudgetDB.Attributes
{
    public class FeatureEnabledAttribute : Attribute, IResourceFilter
    {
        public bool IsEnabled { get; set; }

        public void OnResourceExecuted(ResourceExecutedContext context)
        {
            // Nothing to do here
        }

        public void OnResourceExecuting(ResourceExecutingContext context)
        {
            if (!IsEnabled)
            {
                context.Result = new BadRequestResult();
            }
        }
    }
}
