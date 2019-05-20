namespace Portal.DataLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Portal.Uploads",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UploadedBy = c.String(nullable: false, maxLength: 255),
                        FileName = c.String(nullable: false, maxLength: 255),
                        RandomName = c.String(nullable: false, maxLength: 255),
                        UploadDateTime = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ContentLength = c.Int(nullable: false),
                        ContentType = c.String(nullable: false, maxLength: 25),
                        Timestamp = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "Portal.WeblogComments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CommentDateTime = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        Writer = c.String(nullable: false),
                        Comment = c.String(nullable: false),
                        Timestamp = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                        WeblogPostId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("Portal.WeblogPosts", t => t.WeblogPostId, cascadeDelete: true)
                .Index(t => t.WeblogPostId);
            
            CreateTable(
                "Portal.WeblogPosts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PostDateTime = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        Writer = c.String(nullable: false),
                        Title = c.String(nullable: false),
                        PostContent = c.String(nullable: false),
                        Timestamp = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "Portal.WebsiteVisitors",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        HostAddress = c.String(nullable: false),
                        HostName = c.String(nullable: false),
                        Browser = c.String(nullable: false),
                        Url = c.String(nullable: false),
                        UrlReferrer = c.String(),
                        Timestamp = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("Portal.WeblogComments", "WeblogPostId", "Portal.WeblogPosts");
            DropIndex("Portal.WeblogComments", new[] { "WeblogPostId" });
            DropTable("Portal.WebsiteVisitors");
            DropTable("Portal.WeblogPosts");
            DropTable("Portal.WeblogComments");
            DropTable("Portal.Uploads");
        }
    }
}
