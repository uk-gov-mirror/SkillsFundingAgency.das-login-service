using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SFA.DAS.LoginService.Data.Entities;

namespace SFA.DAS.LoginService.Application.Services
{
    public class CustomSignInManager : SignInManager<LoginUser>
    {
        public CustomSignInManager(UserManager<LoginUser> userManager, IHttpContextAccessor contextAccessor, IUserClaimsPrincipalFactory<LoginUser> claimsFactory, IOptions<IdentityOptions> optionsAccessor, ILogger<SignInManager<LoginUser>> logger, IAuthenticationSchemeProvider schemes) : base(userManager, contextAccessor, claimsFactory, optionsAccessor, logger, schemes)
        {
        }

        public override Task<SignInResult> PasswordSignInAsync(string userName, string password, bool isPersistent, bool lockoutOnFailure)
        {
            var user = UserManager.FindByEmailAsync(userName).Result;

            if (user?.IsEnabled != null && user.IsEnabled == false)
            {
                return Task.FromResult(SignInResult.LockedOut);
            }

            return base.PasswordSignInAsync(userName, password, isPersistent, lockoutOnFailure);
        }
    }
}