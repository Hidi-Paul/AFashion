namespace OCS.DataAccess.Migrations
{
    using OCS.DataAccess.DTO;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;

    internal sealed class Configuration : DbMigrationsConfiguration<Context.FashionContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(Context.FashionContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //

            IList<Category> defaultCategs = new List<Category>();
            IList<Brand> defaultBrands = new List<Brand>();
            IList<Product> defaultProducts = new List<Product>();

            defaultCategs.Add(new Category{ CategoryID = Guid.NewGuid(), CategoryName = "Hat" });
            defaultCategs.Add(new Category{ CategoryID = Guid.NewGuid(), CategoryName = "Shirt" });
            defaultCategs.Add(new Category{ CategoryID = Guid.NewGuid(), CategoryName = "Pants" });
            defaultCategs.Add(new Category{ CategoryID = Guid.NewGuid(), CategoryName = "Shoes" });
            defaultCategs.Add(new Category{ CategoryID = Guid.NewGuid(), CategoryName = "Dress" });

            foreach(var categ in defaultCategs)
            {
                context.Categories.AddOrUpdate(categ);
            }

            defaultBrands.Add(new Brand{ BrandID = Guid.NewGuid(), BrandName = "Lacoste" });
            defaultBrands.Add(new Brand{ BrandID = Guid.NewGuid(), BrandName = "Zara" });
            defaultBrands.Add(new Brand{ BrandID = Guid.NewGuid(), BrandName = "Motivi" });
            defaultBrands.Add(new Brand{ BrandID = Guid.NewGuid(), BrandName = "Berska" });
            defaultBrands.Add(new Brand{ BrandID = Guid.NewGuid(), BrandName = "H&M" });

            foreach(var brand in defaultBrands)
            {
                context.Brands.AddOrUpdate(brand);
            }

            defaultProducts.Add(new Product{ ProductID = Guid.NewGuid(), Brand = defaultBrands[1], Category = defaultCategs[0], ProductPrice = 100, ProductName = "Nice Hat", Image = "http://www.villagehatshop.com/photos/product/giant/4511390S162374/-/size-m.jpg" });
            defaultProducts.Add(new Product{ ProductID = Guid.NewGuid(), Brand = defaultBrands[3], Category = defaultCategs[1], ProductPrice = 20, ProductName = "Good Shirt", Image = "http://scene7.zumiez.com/is/image/zumiez/pdp_hero/Zine-2nd-Inning-Heather-Grey-%26-Marled-Red-Baseball-Shirt-_225749-front.jpg" });
            defaultProducts.Add(new Product{ ProductID = Guid.NewGuid(), Brand = defaultBrands[1], Category = defaultCategs[2], ProductPrice = 40, ProductName = "Grey Pants", Image = "https://www.rei.com/media/d1998f8a-ef3d-4266-9ae3-9469ea553153" });
            defaultProducts.Add(new Product{ ProductID = Guid.NewGuid(), Brand = defaultBrands[2], Category = defaultCategs[3], ProductPrice = 99.9, ProductName = "DVD Boots", Image = "http://files.sharenator.com/67995510_FunnyCrazyScary_shoes-s460x343-10399.jpg" });
            defaultProducts.Add(new Product{ ProductID = Guid.NewGuid(), Brand = defaultBrands[0], Category = defaultCategs[3], ProductPrice = 25, ProductName = "Wooden Boots", Image = "https://m.media-amazon.com/images/G/01/zappos/landing/pages/mensclothing/MelodyTest1/MensShoes6._V506596163_.jpg" });
            defaultProducts.Add(new Product{ ProductID = Guid.NewGuid(), Brand = defaultBrands[4], Category = defaultCategs[4], ProductPrice = 66, ProductName = "Red Hood", Image = "https://s-media-cache-ak0.pinimg.com/736x/58/bf/0c/58bf0c2eff5c91d5908480de69072e89--dresses-on-sale-a-line-dresses.jpg" });
            defaultProducts.Add(new Product{ ProductID = Guid.NewGuid(), Brand = defaultBrands[4], Category = defaultCategs[4], ProductPrice = 60, ProductName = "Blue Ocean Dress", Image = "https://pc-ap.renttherunway.com/productimages/nomodel/1080x/57/AP30.jpg" });

            foreach(var prod in defaultProducts)
            {
                context.Products.AddOrUpdate(prod);
            }

            base.Seed(context);
        }
    }
}
