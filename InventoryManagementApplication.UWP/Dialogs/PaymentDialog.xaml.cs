using InventoryManagementApplication.UWP.ViewModels;
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
    public sealed partial class PaymentDialog : ContentDialog
    {
        public PaymentDialog()
        {
            this.InitializeComponent();
            this.DataContext = new CartViewModel();
        }
        public PaymentDialog(CartViewModel currentViewModel)
        {
            this.InitializeComponent();
            this.DataContext = currentViewModel;
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            (DataContext as CartViewModel).CheckoutAndExit();
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }
    }
}
