using InventoryManagementApplication.UWP.Dialogs;
using Library.InventoryManagement.Models;
using Library.InventoryManagement.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml.Controls;

namespace InventoryManagementApplication.UWP.ViewModels
{
    public class CartViewModel
    {
        private CartService _cartService;
        public StorageFile SelectedCart { get; set; }
        public ItemViewModel SelectedItem { get; set; }
        public double Amount { get; set; }
        public double CheckoutPrice { get; set; }
        public string FileName { get; set; }
        public string PaymentID { get; set; }

        public CartViewModel()
        {
            _cartService = CartService.Current;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void Save()
        {
            int len = _cartService.currentCartName.Length;
            _cartService.Save(_cartService.currentCartName.Remove(len-8,8));
        }

        public ObservableCollection<ItemViewModel> Cart
        {
            get
            {
                if (_cartService == null) return new ObservableCollection<ItemViewModel>();
                // else return new ObservableCollection<ItemViewModel>(_inventoryService.SortResults(SelectedSort).Select(p => new ItemViewModel(p)));
                else return new ObservableCollection<ItemViewModel>(_cartService.Cart.Select(i => new ItemViewModel(i)));
            }
        }

        public ObservableCollection<StorageFile> CartFiles
        {
            get
            {
                if (_cartService == null) return new ObservableCollection<StorageFile>();
                else return new ObservableCollection<StorageFile>(_cartService.CartFiles);
            }
        }

        public async Task New()
        {
            ContentDialog diag = new NewCartDialog();
            await diag.ShowAsync();
            NotifyPropertyChanged("Cart");
        }

        public async Task LoadDiag()
        {
            ContentDialog diag = new LoadCartDialog();
            await _cartService.LoadCarts();
            await diag.ShowAsync();
            NotifyPropertyChanged("Cart");
        }

        public async Task AddDiag()
        {
            ContentDialog diag = new CartDialog(SelectedItem);
            await diag.ShowAsync();
            NotifyPropertyChanged("Inventory");
        }

        public async Task PaymentDiag()
        {
            ContentDialog diag = new PaymentDialog();
            await diag.ShowAsync();
            NotifyPropertyChanged("Cart");
        }
        public async void Update()
        {
            if (SelectedItem != null)
            {
                // check if product or productbyweight
                // replace SelectedProduct with SelectedItem.BoundProduct or SelectedItem.BoundProductByWeight
                var diag = new CartDialog(SelectedItem);
                await diag.ShowAsync();
                NotifyPropertyChanged("Inventory");
            }
        }

        public void Remove()
        {
            var id = SelectedItem?.Id ?? -1;
            if (id >= 1) _cartService.Delete(SelectedItem.Id);
            NotifyPropertyChanged("Cart");
        }

        public void Load()
        {
            _cartService.currentCartName = SelectedCart.DisplayName;
            _cartService.Load(SelectedCart.DisplayName);
            NotifyPropertyChanged("Inventory");
        }

        public async Task Add(ItemType iType)
        {
            ContentDialog diag = new ProductDialog();
            if (iType == ItemType.ProductByWeight)
            {
                // var pbw = new ProductByWeight(SelectedItem as Product);
            }
            else if (iType == ItemType.Product)
            {
                diag = new ProductDialog();
            }
            else if (iType == ItemType.Item)
            {
                diag = new ProductDialog();
            }
            else
            {
                throw new NotImplementedException();
            }
            await diag.ShowAsync();
            NotifyPropertyChanged("Inventory");
        }

        public void Checkout()
        {
            CheckoutPrice = _cartService.CalculatePrice();
        }

        public void CheckoutAndExit()
        {
            var _tmpInventory = InventoryService.Current.Inventory;
            for (int i = 0; i < _cartService.Cart.Count; i++)
            {
                if (_tmpInventory.Contains(_cartService.Cart[i]))
                {
                    InventoryService.Current.Delete(_tmpInventory.FindIndex(p => p == _cartService.Cart[i]));
                    _cartService.Cart.RemoveAt(i);
                }
            }
            
        }

        public enum ItemType
        {
            ProductByWeight, Product, Item
        }

    }
}
