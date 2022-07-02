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
            _cartService.Save(FileName);
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

        public void Load()
        {
            _cartService.Load(SelectedCart.DisplayName);
            NotifyPropertyChanged("Inventory");
        }

    }
}
