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
    public sealed partial class NewCartDialog : ContentDialog
    {
        public NewCartDialog()
        {
            this.InitializeComponent();
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            // var viewModel = DataContext as ProductViewModel;
            /*if ((DataContext as Product).Type == "Quantity") InventoryService.Current.AddOrUpdate(DataContext as Product);
            else
            {
                var pbw = new ProductByWeight(DataContext as Product);
                InventoryService.Current.AddOrUpdate(pbw);
            }*/
            // TODO: use a conversion constructor from view model -> todo

            // InventoryService.Current.AddOrUpdate(DataContext as Product);
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }
    }
}
