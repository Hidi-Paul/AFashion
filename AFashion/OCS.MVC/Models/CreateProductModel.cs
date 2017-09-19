using System;

namespace OCS.MVC.Models
{
    public class CreateProductModel 
    {
        public Guid ID { get; set; }

        public string Name { get; set; }
        public double Price { get; set; }
        public string Brand { get; set; }
        public string Category { get; set; }

        public string ImageExtension { get; set; }
        public byte[] Image { get; set; }
    }
}