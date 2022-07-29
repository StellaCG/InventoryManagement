using Library.InventoryManagement.Utility;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.InventoryManagement.Models
{
    [JsonConverter(typeof(ItemJsonConverter))]
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
            Type = "Weight";
        }

        public ProductByWeight(Product product)
        {
            Name = product.Name;
            Id = product.Id;
            Description = product.Description;
            Price = product.Price;
            Quantity = product.Quantity;
            Weight = product.Quantity;
            Type = "Weight";
        }

        public override string ToString()
        {
            return $"{Id} | {Name} : {Description}. ${Price}, {Weight} units of weight.";
        }
    }
}
