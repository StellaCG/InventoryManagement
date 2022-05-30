using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library.InventoryManagement.Models;


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
            if (amount >= cartContents[index].Quantity)
            {
                cartContents.RemoveAt(index);
            } else
            {
                cartContents[index].Quantity -= amount;
            }
        }
    }
}
