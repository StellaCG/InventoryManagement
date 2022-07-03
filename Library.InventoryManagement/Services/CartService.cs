using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library.InventoryManagement.Models;
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
            cartContents = new List<Item>();
            _cartFiles = new List<StorageFile>();
        }

        public void Add(Product product, double quantity)
        {
            if (product is ProductByWeight)
            {
                (product as ProductByWeight).Weight = quantity;
                Cart.Add(product as ProductByWeight);
            }
            else if (product is Product)
            {
                product.Quantity = (int)quantity;
                Cart.Add(product);
            }
        }

        public void Delete(int index, int amount)
        {
            cartContents.RemoveAt(index);
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
                if (p.GetType() == typeof(ProductByWeight))
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
