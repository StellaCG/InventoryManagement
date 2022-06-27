using System;

namespace Library.InventoryManagement.Models
{
    
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
        public double TotalPrice { get; set; }
        public double Weight { get; set; }
        public bool Bogo { get; set; }

        public Product Parent;

        public Product()
        {

        }

        public Product(Product product)
        {
            Id = product.Id;
            Name = product.Name;
            Description = product.Description;
            Price = product.Price;
            Quantity = product.Quantity;
            Bogo = product.Bogo;
        }

        public override string ToString()
        {
            if (Bogo) return $"{Id} | {Name} : {Description}. ${Price}, {Quantity} units. BOGO";
            else return $"{Id} | {Name} : {Description}. ${Price}, {Quantity} units.";
        }
    }
}
