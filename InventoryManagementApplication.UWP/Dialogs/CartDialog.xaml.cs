using InventoryManagementApplication.UWP.ViewModels;
using Library.InventoryManagement.Models;
using Library.InventoryManagement.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using static InventoryManagementApplication.UWP.ViewModels.CartViewModel;

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace InventoryManagementApplication.UWP.Dialogs
{
    public sealed partial class CartDialog : ContentDialog
    {
        public CartDialog()
        {
            this.InitializeComponent();
            DataContext = new CartViewModel();
        }

        public CartDialog(ItemViewModel selectedIVM)
        {
            this.InitializeComponent();
            this.DataContext = selectedIVM;
        }

        private async void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            var vm = DataContext as ItemViewModel;
            if (vm != null && vm.BoundItem != null)
            {
                if (vm.IsProductByWeight)
                {
                    var cartTmp = vm.BoundProductByWeight;
                    vm.BoundProductByWeight.Weight -= vm.Amount;
                    InventoryService.Current.AddOrUpdate(vm.BoundProductByWeight);
                    CartService.Current.Add(cartTmp, vm.Amount);
                } else if (vm.IsProduct)
                {
                    var cartTmp = vm.BoundProduct;
                    vm.BoundProduct.Quantity -= (int)vm.Amount;
                    InventoryService.Current.AddOrUpdate(vm.BoundProduct);
                    CartService.Current.Add(cartTmp, (int)vm.Amount);
                }
            }
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }
    }
}
