using InventoryManagement.API.Database;
using Library.InventoryManagement.Models;
using Microsoft.AspNetCore.Mvc;

namespace InventoryManagement.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductByWeightController : ControllerBase
    {
        private readonly ILogger<ProductByWeightController> _logger;

        public ProductByWeightController(ILogger<ProductByWeightController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public List<ProductByWeight> Get()
        {
            return FakeDatabase.ProductsByWeight;
        }
    }
}
