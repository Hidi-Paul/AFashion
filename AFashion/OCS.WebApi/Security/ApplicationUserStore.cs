using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using OCS.WebApi.Models;
using OCS.WebApi.SecurityModels;
using System;

namespace OCS.WebApi.Security
{
    public interface IApplicationUserStore : IUserStore<ApplicationUser, Guid>
    {

    }

    public class ApplicationUserStore : UserStore<ApplicationUser, ApplicationRole, Guid, ApplicationUserLogin, ApplicationUserRole, ApplicationUserClaim>, IApplicationUserStore
    {
        public ApplicationUserStore() : base(new SecurityDbContext())
        {

        }

        public ApplicationUserStore(SecurityDbContext context) : base(context)
        {

        }
    }
}