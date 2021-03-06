﻿using Microsoft.AspNet.Identity.EntityFramework;
using OCS.WebApi.SecurityModels;
using System;
using System.Data.Entity;

namespace OCS.WebApi.Security
{
    public class SecurityDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid, ApplicationUserLogin, ApplicationUserRole, ApplicationUserClaim>
    {
        public SecurityDbContext(): base("AFashionDbCon")
        {
            Database.SetInitializer(new CreateDatabaseIfNotExists<SecurityDbContext>());
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        public static SecurityDbContext Create()
        {
            return new SecurityDbContext();
        }
    }
}