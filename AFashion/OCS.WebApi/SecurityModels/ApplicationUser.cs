using Microsoft.AspNet.Identity.EntityFramework;
using System;

namespace OCS.WebApi.SecurityModels
{
    public sealed class ApplicationUser : IdentityUser<Guid,ApplicationUserLogin,ApplicationUserRole,ApplicationUserClaim>
    {

    }
}