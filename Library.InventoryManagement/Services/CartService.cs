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
        private static CartService current;
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

        public static CartService Current
        {
            get
            {
                if (current == null)
                {
                    current = new CartService();
                }
                return current;
            }
        }

        public List<Product> SortResults(int order)
        {
            List<Product> tmpCart = cartContents;
            if (order == 1)
            {
                // sort by name
                tmpCart.Sort(delegate (Product a, Product b)
                {
                    if (a.Name == null && b.Name == null) return 0;
                    else if (a.Name == null) return -1;
                    else if (b.Name == null) return 1;
                    else return a.Name.CompareTo(b.Name);
                });
                return tmpCart;
            }
            else if (order == 2)
            {
                // sort by unit price
                tmpCart.Sort(delegate (Product a, Product b)
                {
                    if (a.TotalPrice == 0 && b.TotalPrice == 0) return 0;
                    else if (a.TotalPrice == 0) return -1;
                    else if (b.TotalPrice == 0) return 1;
                    else return a.TotalPrice.CompareTo(b.TotalPrice);
                });
                return tmpCart;
            }
            else return null;
        }

        public void Save(string fileName)
        {
            var cartJson = JsonConvert.SerializeObject(cartContents
                , new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All });
            File.WriteAllText(fileName, cartJson);
        }

        public void Load(string fileName)
        {
            var cartJson = File.ReadAllText(fileName);
            cartContents = JsonConvert.DeserializeObject<List<Product>>
                (cartJson, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All })
                ?? new List<Product>();
        }

        public double CalculatePrice()
        {
            double checkoutPrice = 0;
            foreach (Product p in cartContents)
            {
                Console.WriteLine(p.GetType());
                if (p.GetType() == typeof(ProductByWeight))
                {
                    checkoutPrice += (p.Weight * p.Price);
                } else
                {
                    checkoutPrice += (p.Quantity * p.Price);
                }
            }
            return checkoutPrice * 1.07;
        }
    }
}
