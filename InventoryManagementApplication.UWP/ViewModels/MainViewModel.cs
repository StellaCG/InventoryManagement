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

namespace InventoryManagementApplication.UWP.ViewModels
{
    class MainViewModel : INotifyPropertyChanged
    {
        public string Query { get; set; }
        public Product SelectedProduct { get; set; }
        private InventoryService _inventoryService;

        public ObservableCollection<Product> Inventory
        {
            get
            {
                if (_inventoryService == null) return new ObservableCollection<Product>();
                if (string.IsNullOrEmpty(Query)) return new ObservableCollection<Product>(_inventoryService.Inventory);
                else
                {
                    return new ObservableCollection<Product>(_inventoryService.Inventory.Where(p => p.Name.ToLower().Contains(Query.ToLower())
                        || p.Description.ToLower().Contains(Query.ToLower())));
                }
                return new ObservableCollection<Product>(_inventoryService.Inventory);
            }
        }

        public MainViewModel()
        {
            _inventoryService = InventoryService.Current;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public async Task Add()
        {
            var diag = new ProductDialog();
            await diag.ShowAsync();
            NotifyPropertyChanged("Inventory");
        }

        public void Remove()
        {
            var id = SelectedProduct?.Id ?? -1;
            if (id >= 1) _inventoryService.Delete(SelectedProduct.Id);
            NotifyPropertyChanged("Inventory");
        }

        public async void Update()
        {
            if (SelectedProduct != null)
            {
                var diag = new ProductDialog(SelectedProduct);
                await diag.ShowAsync();
                NotifyPropertyChanged("Inventory");
            }
        }

        public void Save()
        {
            _inventoryService.Save();
        }

        public void Load()
        {
            _inventoryService.Load();
            NotifyPropertyChanged("Inventory");
        }
        
        public void Refresh()
        {
            NotifyPropertyChanged("Inventory");
        }

    }
}
