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
            if (product.Id <= 0)
            {
                product.Id = FakeDatabase.NextId();
                FakeDatabase.Products.Add(product);
            }

            var itemToUpdate = FakeDatabase.Products.FirstOrDefault(p => p.Id == product.Id);
            if (itemToUpdate == null)
            {
                FakeDatabase.Products.Remove(itemToUpdate);
                FakeDatabase.Products.Add(product);
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
                if (item != null)
                {
                    FakeDatabase.Products.Remove(item);
                }
            }
            return id;
        }
    }
}
