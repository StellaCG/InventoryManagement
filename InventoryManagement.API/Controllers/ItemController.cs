using InventoryManagement.API.Database;
using Library.InventoryManagement.Models;
using Microsoft.AspNetCore.Mvc;

namespace InventoryManagement.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ItemController : ControllerBase
    {
        private readonly ILogger<ItemController> _logger;

        public ItemController(ILogger<ItemController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public List<Item> Get()
        {
            return FakeDatabase.Items;
        }
    }
}
