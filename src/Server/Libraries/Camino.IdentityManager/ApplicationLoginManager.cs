﻿using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authentication;
using Camino.IdentityManager.Contracts;

namespace Camino.IdentityManager
{
    public class ApplicationLoginManager<TUser> : SignInManager<TUser>, ILoginManager<TUser> where TUser : IdentityUser<long>
    {
        public ApplicationLoginManager(UserManager<TUser> userManager, IHttpContextAccessor contextAccessor,
            IUserClaimsPrincipalFactory<TUser> claimsFactory, IOptions<IdentityOptions> optionsAccessor,
            ILogger<SignInManager<TUser>> logger, IAuthenticationSchemeProvider schemes,
            IUserConfirmation<TUser> confirmation) :
            base(userManager, contextAccessor, claimsFactory, optionsAccessor, logger, schemes, confirmation)
        {
        }
    }
}
