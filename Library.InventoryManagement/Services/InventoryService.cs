using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library.InventoryManagement.Models;
using Library.InventoryManagement.Utility;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Library.InventoryManagement.Services
{
    public class InventoryService
    {
        private string persistPath
            = $"{Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)}";
        private static InventoryService current;
        private List<Item> inventoryList;
        public List<Item> Inventory
        {
            get
            {
                return inventoryList;
            }
        }

        public int NextId
        {
            get
            {
                if (!Inventory.Any()) return 0;
                return Inventory.Select(i => i.Id).Max() + 1;
            }
        }

        public void AddOrUpdate(Product product)
        {
            /*if (Inventory.Any(p => (p as Product).Id == product.Id))
            {
                var tmp = Inventory.FindIndex(p => (p as Product).Id == product.Id);
                Inventory[tmp] = product;
            }*/

            if (product.Type == "Quantity" || product.Type == "Weight")
            {
                var response = new WebRequestHandler().Post("http://localhost:5015/Product/AddOrUpdate", product).Result;
                var newProduct = JsonConvert.DeserializeObject<Product>(response);
                var oldVersion = inventoryList.FirstOrDefault(p => p.Id == newProduct.Id);

                if (oldVersion != null)
                {
                    var index = inventoryList.IndexOf(oldVersion);
                    inventoryList.RemoveAt(index);
                    inventoryList.Insert(index, newProduct);
                }
                else
                {
                    inventoryList.Add(newProduct);
                }
            }
            
            /*if (product.Id < 0)
            {
                product.Id = NextId;
                Inventory.Add(product);
            }*/
        }

        public InventoryService()
        {
            var inventoryJson = new WebRequestHandler().Get("http://localhost:5015/Item").Result;
            inventoryList = JsonConvert.DeserializeObject<List<Item>>(inventoryJson);
        }

        public void Create(Item item)
        {
            item.Id = NextId;
            Inventory.Add(item);
        }

        public void Update(Item item)
        {
            
        }

        public void Delete(int index)
        {
            var response = new WebRequestHandler().Get($"http://localhost:5015/Product/Delete/{index}");
            var productDelete = inventoryList.FirstOrDefault(p => p.Id == index);
            if (productDelete == null) return;
            inventoryList.Remove(productDelete);
        }

        public static InventoryService Current
        {
            get
            {
                if (current == null)
                {
                    current = new InventoryService();
                }
                return current;
            }
        }

        public List<Product> SortResults(int order)
        {
            List<Product> tmpInventory = new List<Product>();
            foreach (var p in inventoryList.Where(i => i is Product).ToList())
            {
                tmpInventory.Add(p as Product);
            }
            
            if (order == 0)
            {
                return tmpInventory.OrderBy(p => p.Id).ToList();
            }
            else if (order == 1)
            {
                // sort by name
                tmpInventory.Sort(delegate (Product a, Product b)
                {
                    if (a.Name == null && b.Name == null) return 0;
                    else if (a.Name == null) return -1;
                    else if (b.Name == null) return 1;
                    else return a.Name.CompareTo(b.Name);
                });
                return tmpInventory;
            }
            else if (order == 2)
            {
                // sort by unit price
                tmpInventory.Sort(delegate (Product a, Product b)
                {
                    if (a.Price == 0 && b.Price == 0) return 0;
                    else if (a.Price == 0) return -1;
                    else if (b.Price == 0) return 1;
                    else return a.Price.CompareTo(b.Price);
                });
                return tmpInventory;
            }
            else return null;
        }

        public void Save(string fileName = null)
        {
            if (string.IsNullOrEmpty(fileName)) fileName = $"{persistPath}\\inventoryData.json";
            else fileName = $"{persistPath}\\{fileName}.json";

            var inventoryJson = JsonConvert.SerializeObject(inventoryList
                , new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All });
            File.WriteAllText(fileName, inventoryJson);

        }

        public void Load(string fileName = null)
        {
            if (string.IsNullOrEmpty(fileName)) fileName = $"{persistPath}\\inventoryData.json";
            else fileName = $"{persistPath}\\{fileName}.json";

            var inventoryJson = File.ReadAllText(fileName);
            inventoryList = JsonConvert.DeserializeObject<List<Item>>
                (inventoryJson, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All })
                ?? new List<Item>();

        }
    }
}
