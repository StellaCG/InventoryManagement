using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library.InventoryManagement.Utility;
using Newtonsoft.Json;

namespace Library.InventoryManagement.Models
{
    [JsonConverter(typeof(ItemJsonConverter))]
    public class Item
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int Id { get; set; } = -1;

        public override string ToString()
        {
            return $"{Id} {Name} :: {Description}";
        }
    }
}
