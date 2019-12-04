using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyBudgetDB.Models;
using Microsoft.AspNetCore.Authorization;
using MyBudgetDB.Data;

namespace MyBudgetDB.Authorization
{
    public class CanManageBudgetHandler :
        AuthorizationHandler<CanManageBudgetRequirement, UserBudget>
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public CanManageBudgetHandler(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }
            
        protected override async Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            CanManageBudgetRequirement requirement,
            UserBudget resource)
        {
            var appUser = await _userManager.GetUserAsync(context.User);
            if (appUser == null)
            {
                return;
            }

            if (resource.Owner == appUser.Email)
            {
                context.Succeed(requirement);
            }
        }

    }
}
