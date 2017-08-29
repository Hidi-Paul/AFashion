namespace OCS.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _01Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Brand",
                c => new
                    {
                        BrandID = c.Guid(nullable: false),
                        BrandName = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.BrandID)
                .Index(t => t.BrandName, unique: true);
            
            CreateTable(
                "dbo.Product",
                c => new
                    {
                        ProductID = c.Guid(nullable: false),
                        ProductName = c.String(nullable: false, maxLength: 50),
                        ProductPrice = c.Double(nullable: false),
                        Image = c.String(nullable: false),
                        Brand_BrandID = c.Guid(nullable: false),
                        Category_CategoryID = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.ProductID)
                .ForeignKey("dbo.Brand", t => t.Brand_BrandID, cascadeDelete: true)
                .ForeignKey("dbo.Category", t => t.Category_CategoryID, cascadeDelete: true)
                .Index(t => t.ProductName, unique: true)
                .Index(t => t.Brand_BrandID)
                .Index(t => t.Category_CategoryID);
            
            CreateTable(
                "dbo.Category",
                c => new
                    {
                        CategoryID = c.Guid(nullable: false),
                        CategoryName = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.CategoryID)
                .Index(t => t.CategoryName, unique: true);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Product", "Category_CategoryID", "dbo.Category");
            DropForeignKey("dbo.Product", "Brand_BrandID", "dbo.Brand");
            DropIndex("dbo.Category", new[] { "CategoryName" });
            DropIndex("dbo.Product", new[] { "Category_CategoryID" });
            DropIndex("dbo.Product", new[] { "Brand_BrandID" });
            DropIndex("dbo.Product", new[] { "ProductName" });
            DropIndex("dbo.Brand", new[] { "BrandName" });
            DropTable("dbo.Category");
            DropTable("dbo.Product");
            DropTable("dbo.Brand");
        }
    }
}
