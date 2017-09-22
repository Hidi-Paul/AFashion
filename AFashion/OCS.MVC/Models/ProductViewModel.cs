using System;

namespace OCS.MVC.Models
{
    public class ProductViewModel
    {
        public Guid ID { get; set; }

        public string Name { get; set; }

        public double Price { get; set; }

        public string Brand { get; set; }

        public string Image { get; set; }
    }
}