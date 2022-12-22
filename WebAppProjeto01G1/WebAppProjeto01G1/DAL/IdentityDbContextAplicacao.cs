﻿using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAppProjeto01G1.DAL
{
    public class IdentityDbContextAplicacao : IdentityDbContext
    {
        public IdentityDbContextAplicacao() : base("IdentityDb")
        { 
        }
        static IdentityDbContextAplicacao()
        {
            Database.SetInitializer<IdentityDbContextAplicacao>(new IdentityDbInit());
        }
        public static IdentityDbContextAplicacao Create()
        {
            return new IdentityDbContextAplicacao();
        }
        public class IdentityDbInit : DropCreateDatabaseIfModelChanges<IdentityDbContextAplicacao>
        { 
        }
    }
}