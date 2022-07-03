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

        public string FileName { get; set; }

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
            _cartService.Save(_cartService.currentCartName.TrimEnd("CartData"));
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

        public enum ItemType
        {
            ProductByWeight, Product, Item
        }

    }
}
