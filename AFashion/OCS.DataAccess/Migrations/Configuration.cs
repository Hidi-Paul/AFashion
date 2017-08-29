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

            defaultCategs.Add(new Category{ CategoryID = new Guid("e8137ee9-6204-49c6-bafd-a330165880bb"), CategoryName = "Hat" });
            defaultCategs.Add(new Category{ CategoryID = new Guid("84fb9b82-135b-4b59-aeed-5137cff4298d"), CategoryName = "Shirt" });
            defaultCategs.Add(new Category{ CategoryID = new Guid("7c1a33a0-d9bb-40aa-95c2-ed0adb6f3e5b"), CategoryName = "Pants" });
            defaultCategs.Add(new Category{ CategoryID = new Guid("75914f6e-6f1d-4637-bc6e-f311c2d4737f"), CategoryName = "Shoes" });
            defaultCategs.Add(new Category{ CategoryID = new Guid("e8ce772a-9e35-42b1-b43a-93739a9a9ae0"), CategoryName = "Dress" });
            

            defaultBrands.Add(new Brand{ BrandID = new Guid("0825da82-a2ac-4d08-9149-a44d76cc9952"), BrandName = "Lacoste" });
            defaultBrands.Add(new Brand{ BrandID = new Guid("62d0c4fd-10f6-489f-9a94-420270cb442d"), BrandName = "Zara" });
            defaultBrands.Add(new Brand{ BrandID = new Guid("a3dba29d-85c7-4c61-8f02-50bedb211e57"), BrandName = "Motivi" });
            defaultBrands.Add(new Brand{ BrandID = new Guid("140acce4-e25c-4aa6-8d9d-ff611647450b"), BrandName = "Berska" });
            defaultBrands.Add(new Brand{ BrandID = new Guid("5e96688e-d646-4c30-8346-2e855d25caba"), BrandName = "H&M" });
            

            defaultProducts.Add(new Product{ ProductID = new Guid("ae6484cb-15a9-45ce-8ef9-7915f7aeb045"), Brand = defaultBrands[1], Category = defaultCategs[0], ProductPrice = 100, ProductName = "Nice Hat", Image = "http://www.villagehatshop.com/photos/product/giant/4511390S162374/-/size-m.jpg" });
            defaultProducts.Add(new Product{ ProductID = new Guid("ad864353-9cb5-4610-b336-1d16bd9ad190"), Brand = defaultBrands[3], Category = defaultCategs[1], ProductPrice = 20, ProductName = "Good Shirt", Image = "http://scene7.zumiez.com/is/image/zumiez/pdp_hero/Zine-2nd-Inning-Heather-Grey-%26-Marled-Red-Baseball-Shirt-_225749-front.jpg" });
            defaultProducts.Add(new Product{ ProductID = new Guid("fa208521-ea70-4853-8eb0-69ae9fd94329"), Brand = defaultBrands[1], Category = defaultCategs[2], ProductPrice = 40, ProductName = "Grey Pants", Image = "https://www.rei.com/media/d1998f8a-ef3d-4266-9ae3-9469ea553153" });
            defaultProducts.Add(new Product{ ProductID = new Guid("1949c7c5-462c-4f67-8ade-2d729ee3288c"), Brand = defaultBrands[2], Category = defaultCategs[3], ProductPrice = 99.9, ProductName = "DVD Boots", Image = "http://files.sharenator.com/67995510_FunnyCrazyScary_shoes-s460x343-10399.jpg" });
            defaultProducts.Add(new Product{ ProductID = new Guid("6cfe3107-4d78-4577-a990-77c682f5db27"), Brand = defaultBrands[0], Category = defaultCategs[3], ProductPrice = 25, ProductName = "Wooden Boots", Image = "https://m.media-amazon.com/images/G/01/zappos/landing/pages/mensclothing/MelodyTest1/MensShoes6._V506596163_.jpg" });
            defaultProducts.Add(new Product{ ProductID = new Guid("40271f2b-3a3a-41af-b7d9-4ce3df231d43"), Brand = defaultBrands[4], Category = defaultCategs[4], ProductPrice = 66, ProductName = "Red Hood", Image = "https://s-media-cache-ak0.pinimg.com/736x/58/bf/0c/58bf0c2eff5c91d5908480de69072e89--dresses-on-sale-a-line-dresses.jpg" });
            defaultProducts.Add(new Product{ ProductID = new Guid("39e9c067-71ab-416f-8b09-a0c3007785aa"), Brand = defaultBrands[4], Category = defaultCategs[4], ProductPrice = 60, ProductName = "Blue Ocean Dress", Image = "https://pc-ap.renttherunway.com/productimages/nomodel/1080x/57/AP30.jpg" });

            foreach(var prod in defaultProducts)
            {
                context.Products.AddOrUpdate(prod);
            }

            base.Seed(context);
        }
    }
}
