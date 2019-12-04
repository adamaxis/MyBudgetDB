using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using MyBudgetDB.Data;

namespace MyBudgetDB.Authorization
{
    public class IsBudgetOwnerHandler : AuthorizationHandler<IsBudgetOwnerRequirement, UserBudget>
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public IsBudgetOwnerHandler(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        protected override async Task HandleRequirementAsync(
            AuthorizationHandlerContext context, 
            IsBudgetOwnerRequirement requirement,
            UserBudget resource)
        {
            var appUser = await _userManager.GetUserAsync(context.User);
            if (appUser == null)
            {
                return;
            }

            if (resource.UserId == appUser.Id || context.User.HasClaim(c => c.Type == Claims.IsAdmin)) // admin bypass
            {
                context.Succeed(requirement);
            }
        }
    }
}
