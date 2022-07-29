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
                // check if item is already in cart to pre-correct inventory adjustment
                var inCart = CartService.Current.Cart.FirstOrDefault(i => i.Id == vm.BoundItem.Id);
                if (inCart != null)
                {
                    var product = inCart as Product;
                    if (product.Type == "Weight")
                    {
                        if (vm.Amount < (inCart as ProductByWeight).Weight)
                        {
                            var pbw = inCart as ProductByWeight;
                            var diff = 0 - (pbw.Weight - vm.Amount);
                            pbw.Weight = vm.Amount;
                            var invpbw = InventoryService.Current.Inventory.FirstOrDefault(i => i.Id == product.Id) as ProductByWeight;
                            var updatedInvPbw = invpbw;
                            updatedInvPbw.Weight -= diff;
                            updatedInvPbw.Quantity = (int)updatedInvPbw.Weight;
                            InventoryService.Current.Inventory.Remove(invpbw);
                            InventoryService.Current.Inventory.Add(updatedInvPbw);
                            CartService.Current.Cart.Remove(inCart as ProductByWeight);
                            CartService.Current.Cart.Add(pbw);
                            return;
                        }
                    } else if (product.Type == "Quantity")
                    {
                        if (vm.Amount < product.Quantity)
                        {
                            var tmpprod = product;
                            var diff = 0 - (tmpprod.Quantity - vm.Amount);
                            tmpprod.Quantity = (int)vm.Amount;
                            var invp = InventoryService.Current.Inventory.FirstOrDefault(i => i.Id == product.Id) as Product;
                            var updatedInvP = invp;
                            updatedInvP.Quantity -= (int)diff;
                            InventoryService.Current.Inventory.Remove(invp);
                            InventoryService.Current.Inventory.Add(updatedInvP);
                            CartService.Current.Cart.Remove(product);
                            CartService.Current.Cart.Add(tmpprod);
                            return;
                        }
                    }
                }

                if (vm.IsProductByWeight)
                {
                    // check inventory first
                    var invCheck = (InventoryService.Current.Inventory.FirstOrDefault(p => p.Id == vm.BoundProductByWeight.Id)) as ProductByWeight;
                    if (invCheck.Weight < vm.Amount) return;
                    // remove productbyweight from inventory
                    vm.BoundProductByWeight.Weight -= vm.Amount;
                    vm.BoundProductByWeight.Quantity = (int)vm.BoundProductByWeight.Weight;
                    InventoryService.Current.AddOrUpdate(vm.BoundProductByWeight);
                    // add productbyweight to cart
                    vm.BoundProductByWeight.Weight = Math.Abs(vm.Amount);
                    vm.BoundProductByWeight.Quantity = (int)vm.BoundProductByWeight.Weight;
                    CartService.Current.AddOrUpdate(vm.BoundProductByWeight);
                } else if (vm.IsProduct)
                {
                    // check inventory first
                    var invCheck = (InventoryService.Current.Inventory.FirstOrDefault(p => p.Id == vm.BoundProduct.Id)) as Product;
                    if (invCheck.Quantity < vm.Amount) return;
                    // remove product from inventory
                    vm.BoundProduct.Quantity -= (int)vm.Amount;
                    InventoryService.Current.AddOrUpdate(vm.BoundProduct);
                    // add product to cart
                    vm.BoundProduct.Quantity = Math.Abs((int)vm.Amount);
                    CartService.Current.AddOrUpdate(vm.BoundProduct);
                }
            }
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }
    }
}
