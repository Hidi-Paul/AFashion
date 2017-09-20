using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using OCS.DataAccess.Context;
using OCS.DataAccess.DTO.Security;
using System;

namespace OCS.DataAccess.Repositories.Security
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