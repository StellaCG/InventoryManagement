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

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace InventoryManagementApplication.UWP.Dialogs
{
    public sealed partial class ProductDialog : ContentDialog
    {
        public ProductDialog()
        {
            this.InitializeComponent();
            this.DataContext = new ItemViewModel();
        }

        public ProductDialog(ItemViewModel selectedIVM)
        {
            this.InitializeComponent();
            this.DataContext = selectedIVM;
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            var viewModel = DataContext as ItemViewModel;
            if (viewModel.Type == "Weight")
            {
                var pbw = new ProductByWeight(viewModel.BoundProduct);
                InventoryService.Current.AddOrUpdate(pbw);
            } else if (viewModel.Type == "Quantity")
            {
                InventoryService.Current.AddOrUpdate(viewModel.BoundProduct);
            }
            
            // TODO: use a conversion constructor from view model -> todo

            // InventoryService.Current.AddOrUpdate(DataContext as Product);
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }
    }
}
