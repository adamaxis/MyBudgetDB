using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using MyBudgetDB.Models;

namespace MyBudgetDB.Authorization
{
     //commented out for now, but this will be the authorization filter
    /*public class CanViewBudgetHandler :
        AuthorizationHandler<CanEditPersonRequirement, User>
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public CanViewBudgetHandler(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }
        protected override async Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            CanEditPersonRequirement requirement,
            User resource)
        {
            var appUser = await _userManager.GetUserAsync(context.User);
            if (appUser == null)
            {
                return;
            }

            if ((resource.FirstName == appUser.FirstName && resource.LastName == appUser.LastName) ||
                resource.State == appUser.State ||
                context.User.HasClaim(c => c.Type == "ContentEditor"))
            {
                context.Succeed(requirement);
            }
        }
    }
    */
}