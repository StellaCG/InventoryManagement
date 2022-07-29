using Library.InventoryManagement.Utility;
using Newtonsoft.Json;
using System;

namespace Library.InventoryManagement.Models
{
    [JsonConverter(typeof(ItemJsonConverter))]
    public class Product : Item
    {
        public double Price { get; set; }
        public int Quantity { get; set; }
        public double TotalPrice { get; set; }
        public string Type { get; set; }
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
            Type = product.Type;
            Bogo = product.Bogo;
        }

        public Product(Item item)
        {

        }

        /*if (product is ProductByWeight)
            {
                product.TotalPrice = product.Price * product.Weight;
            }
            else product.TotalPrice = product.Price * product.Quantity;*/

        public override string ToString()
        {
            if (Bogo) return $"{Id} | {Name} : {Description}. ${Price}, {Quantity} units. BOGO";
            else return $"{Id} | {Name} : {Description}. ${Price}, {Quantity} units.";
        }
    }
}
