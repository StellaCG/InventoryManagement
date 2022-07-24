using InventoryManagementApplication.UWP.ViewModels;
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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace InventoryManagementApplication.UWP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CartPage : Page
    {
        public CartPage()
        {
            this.InitializeComponent();
            DataContext = new CartViewModel();
        }

        private void Nav_Back_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.GoBack();
        }
        private void Save_Cart_Click(object sender, RoutedEventArgs e)
        {
            (DataContext as CartViewModel).Save();
        }

        private async void Checkout_Click(object sender, RoutedEventArgs e)
        {
            var vm = (DataContext as CartViewModel);
            vm.Checkout();
            await vm.PaymentDiag();
        }
    }
}
