using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library.InventoryManagement.Models;
using Library.InventoryManagement.Utility;
using Newtonsoft.Json;
using Windows.Storage;

namespace Library.InventoryManagement.Services
{
    public class CartService
    {
        private string persistPath
            = $"{Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)}";
        private static CartService current;
        public string currentCartName;
        private List<Item> cartContents;
        private List<StorageFile> _cartFiles;
        public List<Item> Cart
        {
            get
            {
                return cartContents;
            }
        }

        public List<StorageFile> CartFiles
        {
            get
            {
                return _cartFiles;
            }
        }

        public CartService()
        {
            var cartJson = new WebRequestHandler().Get("http://localhost:5015/Cart").Result;
            cartContents = JsonConvert.DeserializeObject<List<Item>>(cartJson);
            _cartFiles = new List<StorageFile>();
        }

        public void AddOrUpdate(Product product)
        {
            var response = new WebRequestHandler().Post("http://localhost:5015/Cart/AddOrUpdate", product).Result;
            var newProduct = JsonConvert.DeserializeObject<Product>(response);
            if (newProduct.Quantity <= 0) return;
            var oldVersion = cartContents.FirstOrDefault(p => p.Id == newProduct.Id);
            
            if (oldVersion != null)
            {
                var index = cartContents.IndexOf(oldVersion);
                cartContents.RemoveAt(index);
                cartContents.Insert(index, newProduct);
            } else
            {
                cartContents.Add(newProduct);
            }
        }

        public void Delete(int index)
        {
            var response = new WebRequestHandler().Get($"http://localhost:5015/Cart/Delete/{index}");
            var productDelete = cartContents.FirstOrDefault(p => p.Id == index);
            var productUpdate = InventoryService.Current.Inventory.FirstOrDefault(p => p.Id == index);
            if (productUpdate != null)
            {
                var product = productUpdate as Product;
                if (product.Type == "Weight")
                {
                    var tmpUpdate = productUpdate as ProductByWeight;
                    tmpUpdate.Weight += (productDelete as ProductByWeight).Weight;
                    InventoryService.Current.Inventory.Remove(product);
                    InventoryService.Current.Inventory.Add(tmpUpdate);
                } else if (product.Type == "Quantity")
                {
                    var tmpUpdate = product;
                    tmpUpdate.Quantity += (productDelete as Product).Quantity;
                    InventoryService.Current.Inventory.Remove(product);
                    InventoryService.Current.Inventory.Add(tmpUpdate);
                }
            }
            if (productDelete == null) return;
            cartContents.Remove(productDelete);
        }

        public void DeletePermanently(int index)
        {
            var response = new WebRequestHandler().Get($"http://localhost:5015/Cart/DeletePermanently/{index}");
            var productDelete = cartContents.FirstOrDefault(p => p.Id == index);
            if (productDelete != null) cartContents.Remove(productDelete);
            else return;
        }

        public static CartService Current
        {
            get
            {
                if (current == null)
                {
                    current = new CartService();
                }
                return current;
            }
        }
        private async Task GetAllCarts(StorageFolder folder)
        {
            var carts = await folder.GetItemsAsync();
            foreach (var cart in carts)
            {
                if (cart is StorageFile && cart.Name.Contains("CartData") && !_cartFiles.Any(c => c.DisplayName == (cart as StorageFile).DisplayName)) _cartFiles.Add(cart as StorageFile);
            }
        }

        public List<Product> SortResults(int order)
        {
            List<Product> tmpCart = new List<Product>();
            foreach (var p in cartContents.Where(i => i is Product).ToList())
            {
                tmpCart.Add(p as Product);
            }

            if (order == 1)
            {
                // sort by name
                tmpCart.Sort(delegate (Product a, Product b)
                {
                    if (a.Name == null && b.Name == null) return 0;
                    else if (a.Name == null) return -1;
                    else if (b.Name == null) return 1;
                    else return a.Name.CompareTo(b.Name);
                });
                return tmpCart;
            }
            else if (order == 2)
            {
                // sort by unit price
                tmpCart.Sort(delegate (Product a, Product b)
                {
                    if (a.TotalPrice == 0 && b.TotalPrice == 0) return 0;
                    else if (a.TotalPrice == 0) return -1;
                    else if (b.TotalPrice == 0) return 1;
                    else return a.TotalPrice.CompareTo(b.TotalPrice);
                });
                return tmpCart;
            }
            else return null;
        }

        public void Save(string fileName)
        {
            if (string.IsNullOrEmpty(fileName)) fileName = $"{persistPath}\\defaultCartData.json";
            else fileName = $"{persistPath}\\{fileName}CartData.json";

            var cartJson = JsonConvert.SerializeObject(cartContents
                , new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All });
            File.WriteAllText(fileName, cartJson);
        }

        public void Load(string fileName = null)
        {
            if (string.IsNullOrEmpty(fileName)) fileName = $"{persistPath}\\defaultCartData.json";
            else fileName = $"{persistPath}\\{fileName}.json";

            var cartJson = File.ReadAllText(fileName);
            cartContents = JsonConvert.DeserializeObject<List<Item>>
                (cartJson, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All })
                ?? new List<Item>();
        }

        public async Task LoadCarts()
        {
            StorageFolder folder = await StorageFolder.GetFolderFromPathAsync(@persistPath);
            await GetAllCarts(folder);
        }

        public double CalculatePrice()
        {
            double checkoutPrice = 0;
            foreach (Product p in cartContents)
            {
                // Console.WriteLine(p.GetType());
                if (p.Type == "Weight")
                {
                    checkoutPrice += ((p as ProductByWeight).Weight * p.Price);
                }
                else
                {
                    if (p.Bogo)
                    {
                        checkoutPrice += (((p.Quantity / 2) + (p.Quantity % 2)) * p.Price);
                    }
                    else checkoutPrice += (p.Quantity * p.Price);
                }
            }
            return checkoutPrice * 1.07;
        }
    }
}
