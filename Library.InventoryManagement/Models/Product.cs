using System;

namespace Library.InventoryManagement.Models
{
    public class Product
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
        public double TotalPrice { get; set; }

        public Product Parent;

        public Product()
        {

        }

        public Product(Product product)
        {
            Name = product.Name;
            Description = product.Description;
            Price = product.Price;
            Quantity = product.Quantity;
        }

        public override string ToString()
        {
            return $"{Name} : {Description}. ${Price}, {Quantity} units.";
        }
    }
}
