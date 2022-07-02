using Library.InventoryManagement.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagementApplication.UWP.ViewModels
{
    public class ItemViewModel : INotifyPropertyChanged
    {
        public string Name
        {
            get
            {
                return BoundItem?.Name ?? string.Empty;
            }
            set
            {
                if (BoundItem == null) return;
                BoundItem.Name = value;
            }
        }
        
        public string Description
        {
            get
            {
                return BoundItem?.Description ?? string.Empty;
            }
            set
            {
                if (BoundItem == null) return;
                BoundItem.Description = value;
            }
        }

        public string Type
        {
            get
            {
                if (IsProduct) return BoundProduct?.Type ?? string.Empty;
                else if (IsProductByWeight) return BoundProductByWeight?.Type ?? string.Empty;
                else return string.Empty;
            }
        }

        public double Price
        {
            get
            {
                if (IsProduct) return BoundProduct?.Price ?? 0;
                else if (IsProductByWeight) return BoundProductByWeight?.Price ?? 0;
                else return 0;
            }
        }

        public int Quantity
        {
            get
            {
                if (IsProduct) return BoundProduct?.Quantity ?? 0;
                else return 0;
            }
        }

        public double Weight
        {
            get
            {
                if (IsProductByWeight) return BoundProductByWeight?.Weight ?? 0;
                else if (IsProduct) return BoundProduct?.Quantity ?? 0;
                else return 0;
            }
        }

        public bool Bogo
        {
            get
            {
                if (IsProductByWeight) return BoundProductByWeight?.Bogo ?? false;
                else if (IsProduct) return BoundProduct?.Bogo ?? false;
                else return false;
            }
        }

        public int Id
        {
            get
            {
                return BoundItem?.Id ?? 0;
            }
        }

        public Item BoundItem
        {
            get
            {
                if (BoundProduct != null) return BoundProduct;
                return BoundProductByWeight;
            }
        }

        public bool IsProduct { get { return BoundProduct != null; } }
        public bool IsProductByWeight {  get { return BoundProductByWeight != null; } }

        private Product boundProduct;
        public Product BoundProduct
        {
            get
            {
                return boundProduct;
            }
        }

        private ProductByWeight boundProductByWeight;
        public ProductByWeight BoundProductByWeight
        {
            get
            {
                return boundProductByWeight;
            }
        }

        public ItemViewModel()
        {
            boundProductByWeight = null;
            boundProduct = new Product();
        }

        public ItemViewModel(Item i)
        {
            if (i == null) return;
            else if (i is ProductByWeight) boundProductByWeight = i as ProductByWeight;
            else if (i is Product) boundProduct = i as Product;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public override string ToString()
        {
            return $"{Id} {Name} :: {Description}";
        }
    }
}
