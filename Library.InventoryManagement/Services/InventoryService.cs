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
    public class InventoryService
    {
        private List<Product> inventoryList;
        public List<Product> Inventory
        {
            get
            {
                return inventoryList;
            }
        }

        public InventoryService()
        {
            inventoryList = new List<Product>();
        }

        public void Create(Product product)
        {
            Inventory.Add(product);
        }

        public void Update(Product product)
        {

        }

        public void Delete(int index)
        {
            inventoryList.RemoveAt(index);
        }

        public void Save(string fileName)
        {
            var todosJson = JsonConvert.SerializeObject(inventoryList);
            File.WriteAllText(fileName, todosJson);
        }

        public void Load(string fileName)
        {
            var todosJson = File.ReadAllText(fileName);
            inventoryList = JsonConvert.DeserializeObject<List<Product>>(todosJson) ?? new List<Product>();
        }
    }
}
