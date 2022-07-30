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
using Windows.UI.Xaml.Controls;

namespace InventoryManagementApplication.UWP.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public string Query { get; set; }
        public ItemViewModel SelectedItem { get; set; }
        public int SelectedSort { get; set; }
        private InventoryService _inventoryService;

        public ObservableCollection<ItemViewModel> Inventory
        {
            get
            {
                if (_inventoryService == null) return new ObservableCollection<ItemViewModel>();
                if (string.IsNullOrEmpty(Query))
                {
                    return new ObservableCollection<ItemViewModel>(_inventoryService.SortResults(SelectedSort).Select(p => new ItemViewModel(p)));
                }
                else
                {
                    return new ObservableCollection<ItemViewModel>(_inventoryService.Inventory.Select(i=> new ItemViewModel(i)).
                        Where(i => i.Name.ToLower().Contains(Query.ToLower())
                        || i.Description.ToLower().Contains(Query.ToLower())));
                }
                // return new ObservableCollection<ItemViewModel>(_inventoryService.Inventory.Select(i => new ItemViewModel(i)));
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

        public async Task Add(ItemType iType)
        {
            ContentDialog diag = null;
            if (iType == ItemType.ProductByWeight)
            {
                diag = new ProductDialog();
            } else if (iType == ItemType.Product)
            {
                diag = new ProductDialog();
            } else if (iType == ItemType.Item)
            {
                diag = new ProductDialog();
            } else
            {
                throw new NotImplementedException();
            }
            await diag.ShowAsync();
            NotifyPropertyChanged("Inventory");
        }

        public void Remove()
        {
            var id = SelectedItem?.Id ?? -1;
            if (id >= 1) _inventoryService.Delete(SelectedItem.Id);
            NotifyPropertyChanged("Inventory");
        }

        public async void Update()
        {
            if (SelectedItem != null)
            {
                // check if product or productbyweight
                // replace SelectedProduct with SelectedItem.BoundProduct or SelectedItem.BoundProductByWeight
                var diag = new ProductDialog(SelectedItem);
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

        public async Task Add_To_Cart(ItemViewModel selection)
        {
            var vm = new CartViewModel();
            vm.SelectedItem = selection;
            await vm.AddDiag();
            NotifyPropertyChanged("Inventory");
        }

        public enum ItemType
        {
            ProductByWeight, Product, Item
        }

    }
}
