using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagementApplication.UWP.ViewModels
{
    class ProductViewModel : INotifyPropertyChanged
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
        public double TotalPrice { get; set; }
        public double Weight { get; set; }
        public bool Bogo { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public override string ToString()
        {
            if (Bogo) return $"{Id} | {Name} : {Description}. ${Price}, {Quantity} units. BOGO";
            else return $"{Id} | {Name} : {Description}. ${Price}, {Quantity} units.";
        }
    }
}
