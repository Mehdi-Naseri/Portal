using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;


using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Portal.Models;
using System.Threading.Tasks;

using Portal.DataLayer.DataContext;
using Portal.DataLayer.Migrations;
using Portal.DomainClasses.Models;
using System.Data.Entity;


namespace Portal
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            InitializePortalDatabase();
            //موقتا غیر فعال شده است
            CreateAdminUserAndRole();
        }

        protected void Session_Start()
        {
            //موقتا غیر فعال شده است
            SaveVisitorsInformation();
        }

        private void InitializePortalDatabase()
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<PortalDb, Configuration>());
            var a = new PortalDb();
            a.Database.Initialize(true);

            //Database.SetInitializer<IdentityDbContext>(null);
        }
        private void CreateAdminUserAndRole()
        {
            //Initialzie Admin user and Password
            IdentityDbContext IdentityDbContext1 = new IdentityDbContext();
            UserManager<ApplicationUser> UserManager1 = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            //Create User "Admin" if it is not existed.
            if (UserManager1.FindByName("Admin") == null)
            {
                var user = new ApplicationUser() { UserName = "Admin" };
                UserManager1.Create(user, "Pass#1");
            }
            //Create Role "Admin" if it is not existed.
            RoleManager<IdentityRole> RoleManager1 = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new ApplicationDbContext()));
            if (RoleManager1.FindByName("Admin") == null)
            {
                IdentityRole IdentityRole1 = new IdentityRole("Admin");
                RoleManager1.Create(IdentityRole1);
            }
            //Add Admin user to Admin Role
            string IdentityUserId = IdentityDbContext1.Users.First(x => x.UserName == "Admin").Id;
            UserManager1.AddToRole(IdentityUserId, "Admin");
        }
        private void SaveVisitorsInformation()
        {
            using (PortalDb db = new PortalDb())
            {
                WebsiteVisitor WebsiteVisitor1 = new WebsiteVisitor();
                WebsiteVisitor1.Date = DateTime.Now;
                WebsiteVisitor1.HostAddress = Request.UserHostAddress;
                WebsiteVisitor1.HostName = Request.UserHostName;
                WebsiteVisitor1.Browser = Request.Browser.Browser;
                WebsiteVisitor1.Url = Request.Url.AbsoluteUri;
                if (Request.UrlReferrer != null)
                    WebsiteVisitor1.UrlReferrer = Request.UrlReferrer.AbsoluteUri;
                else
                    WebsiteVisitor1.UrlReferrer = "";
                db.WebsiteVisitors.Add(WebsiteVisitor1);
                db.SaveChanges();
            }
        }
    }
}
