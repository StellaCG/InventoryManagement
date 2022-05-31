using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library.InventoryManagement.Models;
using Newtonsoft.Json;

namespace Library.InventoryManagement.Services
{
    public class CartService
    {
        private List<Product> cartContents;
        public List<Product> Cart
        {
            get
            {
                return cartContents;
            }
        }

        public CartService()
        {
            cartContents = new List<Product>();
        }

        public void Add(Product product, int quantity)
        {
            product.Quantity = quantity;
            Cart.Add(product);
        }

        public void Delete(int index, int amount)
        {
            cartContents.RemoveAt(index);
        }

        public void Save(string fileName)
        {
            var todosJson = JsonConvert.SerializeObject(cartContents);
            File.WriteAllText(fileName, todosJson);
        }

        public void Load(string fileName)
        {
            var todosJson = File.ReadAllText(fileName);
            cartContents = JsonConvert.DeserializeObject<List<Product>>(todosJson) ?? new List<Product>();
        }
    }
}
