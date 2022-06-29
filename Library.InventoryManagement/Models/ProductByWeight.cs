using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.InventoryManagement.Models
{
    public class ProductByWeight : Product
    {
        public new double Weight { get; set; }

        public ProductByWeight() { }
        public ProductByWeight(ProductByWeight product)
        {
            Name = product.Name;
            Id = product.Id;
            Description = product.Description;
            Price = product.Price;
            Quantity = product.Quantity;
            Weight = product.Weight;
        }

        public ProductByWeight(Product product)
        {
            Name = product.Name;
            Id = product.Id;
            Description = product.Description;
            Price = product.Price;
            Quantity = 0;
            Weight = product.Quantity;
        }

        public override string ToString()
        {
            return $"{Id} | {Name} : {Description}. ${Price}, {Weight} units of weight.";
        }
    }
}
