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
    public class ProductViewModel : INotifyPropertyChanged
    {
        public int Id
        {
            get
            {
                return BoundProduct?.Id ?? 0;
            }
        }
        public string Name
        {
            get
            {
                return BoundProduct?.Name ?? BoundProductByWeight.Name ?? string.Empty;
            }
            set
            {
                if (BoundProduct == null) return;
                BoundProduct.Name = value;
            }
        }
        public string Description
        {
            get
            {
                return BoundProduct?.Description ?? BoundProductByWeight.Description ?? string.Empty;
            }
            set
            {
                BoundProduct.Description = value;
            }
        }
        public double Price { get; set; }
        public int Quantity { get; set; }
        public double TotalPrice { get; set; }
        public double Weight { get; set; }
        public string Type { get; set; }
        public bool Bogo { get; set; }

        public ProductViewModel()
        {
            boundProductByWeight = null;
            boundProduct = new Product();
        }
        
        public ProductViewModel(Product p)
        {
            if (p == null) return;
            else if (p is ProductByWeight)
            {
                boundProductByWeight = p as ProductByWeight;
            } else if (p is Product)
            {
                boundProduct = p as Product;
            }
        }

        public bool IsProduct()
        {
            return (BoundProduct != null);
        }

        public bool IsProductByWeight()
        {
            return (BoundProductByWeight != null);
        }

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
                return BoundProductByWeight;
            }
        }

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
