using InventoryManagement.API.Database;
using Library.InventoryManagement.Models;
using Microsoft.AspNetCore.Mvc;

namespace InventoryManagement.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CartController : ControllerBase
    {
        private readonly ILogger<CartController> _logger;

        public CartController(ILogger<CartController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public List<Item> Get()
        {
            return FakeCart.CartItems;
        }

        [HttpPost("AddOrUpdate")]
        public Product AddOrUpdate(Product product)
        {
            if (product.Quantity <= 0)
            {
                Delete(product.Id);
                return product;
            }

            if (product.Type == "Weight")
            {
                var itemToUpdate = FakeCart.CartProductsByWeight.FirstOrDefault(p => p.Id == product.Id);
                if (itemToUpdate != null)
                {
                    FakeCart.CartProductsByWeight.Remove(itemToUpdate);
                    FakeCart.CartProductsByWeight.Add(product as ProductByWeight);
                } else
                {
                    FakeCart.CartProductsByWeight.Add(product as ProductByWeight);
                }
            } else if (product.Type == "Quantity")
            {
                var itemToUpdate = FakeCart.CartProducts.FirstOrDefault(p => p.Id == product.Id);
                if (itemToUpdate != null)
                {
                    FakeCart.CartProducts.Remove(itemToUpdate);
                    FakeCart.CartProducts.Add(product);
                } else
                {
                    FakeCart.CartProducts.Add(product);
                }
            }
            return product;
        }

        [HttpGet("Delete/{id}")]
        public int Delete(int id)
        {
            var itemToDelete = FakeCart.CartItems.FirstOrDefault(i => i.Id == id);
            var itemToUpdate = FakeDatabase.Items.FirstOrDefault(i => i.Id == id);
            if (itemToUpdate != null)
            {
                var item = itemToUpdate as Product;
                if (item.Type == "Weight")
                {
                    var tmpUpdate = FakeDatabase.ProductsByWeight.FirstOrDefault(p => p.Id == item.Id);
                    tmpUpdate.Weight += (itemToDelete as ProductByWeight).Weight;
                    FakeDatabase.ProductsByWeight.Remove(itemToUpdate as ProductByWeight);
                    FakeDatabase.ProductsByWeight.Add(tmpUpdate);
                } else if (item.Type == "Quantity")
                {
                    var tmpUpdate = FakeDatabase.Products.FirstOrDefault(p => p.Id == item.Id);
                    tmpUpdate.Quantity += (itemToDelete as Product).Quantity;
                    FakeDatabase.Products.Remove(item);
                    FakeDatabase.Products.Add(tmpUpdate);
                }
            }
            if (itemToDelete != null)
            {
                var item = itemToDelete as Product;
                if (item.Type == "Weight") FakeCart.CartProductsByWeight.Remove(item as ProductByWeight);
                else if (item.Type == "Quantity") FakeCart.CartProducts.Remove(item);
            }
            return id;
        }

        [HttpGet("DeletePermanently/{id}")]
        public int DeletePermanently(int id)
        {
            var itemToDelete = FakeCart.CartItems.FirstOrDefault(i => i.Id == id);
            if (itemToDelete != null)
            {
                var prod = itemToDelete as Product;
                if (prod.Type == "Weight") FakeCart.CartProductsByWeight.Remove(prod as ProductByWeight);
                else if (prod.Type == "Quantity") FakeCart.CartProducts.Remove(prod);
                return id;
            }
            return id;
        }
    }
}
