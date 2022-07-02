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
using static InventoryManagementApplication.UWP.ViewModels.MainViewModel;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace InventoryManagementApplication.UWP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            DataContext = new MainViewModel();
        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            (DataContext as MainViewModel).Refresh();
        }

        private async void Add_Click(object sender, RoutedEventArgs e)
        {
            var vm = DataContext as MainViewModel;
            if (vm != null) await vm.Add(ItemType.Product);
        }

        private void Remove_Click(object sender, RoutedEventArgs e)
        {
            var vm = DataContext as MainViewModel;
            if (vm != null) vm.Remove();
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            var vm = DataContext as MainViewModel;
            if (vm != null) vm.Update();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            (DataContext as MainViewModel).Save();
        }

        private void Load_Click(object sender, RoutedEventArgs e)
        {
            (DataContext as MainViewModel).Load();

        }

        private async void New_Cart_Click(object sender, RoutedEventArgs e)
        {
            var vm = new CartViewModel();
            await vm.New();
        }

        private async void Load_Cart_Click(object sender, RoutedEventArgs e)
        {
            var vm = new CartViewModel();
            await vm.Load();
        }

        private void SortComboBox_SelectionChanged(object sender, RoutedEventArgs e)
        {
            (DataContext as MainViewModel).Refresh(); 
        }

    }
}
