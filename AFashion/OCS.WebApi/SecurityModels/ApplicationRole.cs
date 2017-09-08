using Microsoft.AspNet.Identity.EntityFramework;
using System;

namespace OCS.WebApi.SecurityModels
{
    public class ApplicationRole : IdentityRole<Guid, ApplicationUserRole>
    {

    }
}