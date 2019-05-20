using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data.Entity;
using Portal.DomainClasses.Models;

namespace Portal.DataLayer.DataContext
{
    public class PortalDb : DbContext
    {
        public PortalDb()
            : base("name=PortalContextConnection")
        {
        }
        public DbSet<Upload> Uploads { get; set; }
        public DbSet<WeblogComment> WeblogComments { get; set; }
        public DbSet<WeblogPost> WeblogPosts { get; set; }
        public DbSet<WebsiteVisitor> WebsiteVisitors { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //جهت ذخیره داده های فارسی در پایگاه داده اضافه شده است.
            modelBuilder.Properties<DateTime>().Configure(c => c.HasColumnType("datetime2"));
            //جهت ذخیره جداول در شمای "پورتال" اضافه شده است.
            modelBuilder.HasDefaultSchema("Portal");
            base.OnModelCreating(modelBuilder);
        }
    }
}
