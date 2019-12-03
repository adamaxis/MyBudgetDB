using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MyBudgetDB.Attributes;
using MyBudgetDB.Authorization;
using MyBudgetDB.Data;
using MyBudgetDB.Models.AccountViewModels;
using MyBudgetDB.Services;

namespace MyBudgetDB.Api
{
    [HandleException]
    [Route("api/AccountApi")]
    public class AccountApiController : Controller
    {
        private readonly BudgetService _budgetService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly AppSecrets _appSettings;
        private readonly ILogger _log;

        public AccountApiController(
            BudgetService service,
            ILogger<BudgetApiController> log,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _budgetService = service;
            _log = log;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody]LoginViewModel model)
        {
            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);
            if (!result.Succeeded)
            {
                return BadRequest(new { message = "Username or password is incorrect" });
                //return Unauthorized();
            }

            //var tokenHandler = new JwtSecurityTokenHandler(); //A SecurityTokenHandler designed for creating and validating Json Web Tokens
            //var pass = Encoding.ASCII.GetBytes(_appSettings.Password); //Encodes a set of characters into a sequence of bytes.
            //var secretUser = Encoding.ASCII.GetBytes(_appSettings.User);
            //var tokenDescriptor = new SecurityTokenDescriptor
            //{
            //    Subject = new ClaimsIdentity(new Claim[]
            //    {
            //        new Claim(ClaimTypes.Name, user.Id.ToString())
            //    }),
            //    Expires = DateTime.UtcNow.AddDays(7),
            //    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            //};

            //}

            // If we got this far, something failed, redisplay form
            //var user = await _userService.GetUserAsync(User);
            return Ok(new { message = "you are logged" });
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody]RegisterViewModel model)
        {
            try
            {
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                    await AddClaims(model, user);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [Authorize]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            _log.LogInformation("User logged out.");
            return Ok(new{message = "You are logged out."});
        }

        private async Task AddClaims(RegisterViewModel model, ApplicationUser user)
        {
            if (user.UserName == "valatorre@dmacc.edu" || user.UserName == "ddraper@dmacc.edu" || user.UserName == "lrussell@dmacc.edu")
            {
                var isAdmin = new Claim(Claims.IsAdmin, "true", ClaimValueTypes.Boolean);
                await _userManager.AddClaimAsync(user, isAdmin);
            }

            var isActive = new Claim(Claims.IsActive, "true", ClaimValueTypes.Boolean);
            await _userManager.AddClaimAsync(user, isActive);
        }
    }
}