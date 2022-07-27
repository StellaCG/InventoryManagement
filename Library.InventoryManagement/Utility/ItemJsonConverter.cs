using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library.InventoryManagement.Models;
using Newtonsoft.Json.Linq;

namespace Library.InventoryManagement.Utility
{
    public class ItemJsonConverter : JsonCreationConverter<Item>
    {
        protected override Item Create(Type objectType, JObject jObject)
        {
            if (jObject == null) throw new ArgumentException("jObject");

            if (jObject["weight"] != null || jObject["Weight"] != null)
            {
                return new InventoryManagement.Models.ProductByWeight();
            } else if (jObject["quantity"] != null || jObject["Quantity"] != null)
            {
                return new Product();
            } else
            {
                return new Item();
            }
        }
    }
}
