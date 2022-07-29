using InventoryManagement.API.Database;
using Library.InventoryManagement.Models;
using Microsoft.AspNetCore.Mvc;

namespace InventoryManagement.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly ILogger<ProductController> _logger;

        public ProductController(ILogger<ProductController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public List<Product> Get()
        {
            return FakeDatabase.Products;
        }

        [HttpPost("AddOrUpdate")]
        public Product AddOrUpdate(Product product)
        {
            if (product.Quantity <= 0)
            {
                Delete(product.Id);
                return product;
            }

            if (product.Id <= 0)
            {
                product.Id = FakeDatabase.NextId();
                if (product.Type == "Quantity")
                {
                    FakeDatabase.Products.Add(product);
                }
                else if (product.Type == "Weight")
                {
                    FakeDatabase.ProductsByWeight.Add(product as ProductByWeight);
                }
            }
            else if (product.Type == "Quantity")
            {
                var itemToUpdate = FakeDatabase.Products.FirstOrDefault(p => p.Id == product.Id);
                if (itemToUpdate != null)
                {
                    FakeDatabase.Products.Remove(itemToUpdate);
                    FakeDatabase.Products.Add(product);
                }
            } else if (product.Type == "Weight")
            {
                var itemToUpdate = FakeDatabase.ProductsByWeight.FirstOrDefault(p => p.Id == product.Id);
                if (itemToUpdate != null)
                {
                    FakeDatabase.ProductsByWeight.Remove(itemToUpdate);
                    FakeDatabase.ProductsByWeight.Add(product as ProductByWeight);
                }
            }

            return product;
        }

        [HttpGet("Delete/{id}")]
        public int Delete(int id)
        {
            var itemToDelete = FakeDatabase.Items.FirstOrDefault(i => i.Id == id);
            if (itemToDelete != null)
            {
                var item = itemToDelete as Product;
                if (item.Type == "Weight")
                {
                    FakeDatabase.ProductsByWeight.Remove(item as ProductByWeight);
                } else if (item.Type == "Quantity")
                {
                    FakeDatabase.Products.Remove(item);
                }
            }
            return id;
        }
    }
}
