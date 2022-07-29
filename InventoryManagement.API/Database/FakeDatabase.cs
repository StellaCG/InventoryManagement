using Library.InventoryManagement.Models;

namespace InventoryManagement.API.Database
{
    public static class FakeDatabase
    {
        public static List<Item> Items
        {
            get
            {
                var returnList = new List<Item>();
                Products.ForEach(returnList.Add);
                ProductsByWeight.ForEach(returnList.Add);
                return returnList;
            }
        }

        public static List<Product> Products = new List<Product>
        {
            new Product { Name = "Test Product1", Description = "Description1", Id = 1, Quantity = 10, Price = 3, Type = "Quantity"},
            new Product { Name = "Test Product2", Description = "Description2", Id = 2, Quantity = 10, Price = 3, Type = "Quantity"},
            new Product { Name = "Test Product3", Description = "Description3", Id = 3, Quantity = 10, Price = 3, Type = "Quantity"},
            new Product { Name = "Test Product4", Description = "Description4", Id = 4, Quantity = 10, Price = 3, Type = "Quantity"},
            new Product { Name = "Test Product5", Description = "Description5", Id = 5, Quantity = 10, Price = 3, Type = "Quantity", Bogo = true}
        };

        public static List<ProductByWeight> ProductsByWeight = new List<ProductByWeight>
        {
            new ProductByWeight { Name = "Test ProductByWeight1", Description = "Description1", Id = 6, Weight = 10, Price = 5, Type = "Weight"},
            new ProductByWeight { Name = "Test ProductByWeight2", Description = "Description2", Id = 7, Weight = 10, Price = 5, Type = "Weight"},
            new ProductByWeight { Name = "Test ProductByWeight3", Description = "Description3", Id = 8, Weight = 10, Price = 5, Type = "Weight"},
            new ProductByWeight { Name = "Test ProductByWeight4", Description = "Description4", Id = 9, Weight = 10, Price = 5, Type = "Weight", Bogo = true}
        };

        public static int NextId()
        {
            if (!Items.Any()) return 1;
            return Items.Select(i => i.Id).Max() + 1;
        }
    }
}
