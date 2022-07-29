using Library.InventoryManagement.Models;

namespace InventoryManagement.API.Database
{
    public static class FakeCart
    {
        public static List<Item> CartItems
        {
            get
            {
                var returnList = new List<Item>();
                CartProducts.ForEach(returnList.Add);
                CartProductsByWeight.ForEach(returnList.Add);
                return returnList;
            }
        }

        public static List<Product> CartProducts = new List<Product>{};

        public static List<ProductByWeight> CartProductsByWeight = new List<ProductByWeight>{};

        // NOTE TO SELF: remove later if not needed
        public static int NextId()
        {
            if (!CartItems.Any()) return 1;
            return CartItems.Select(i => i.Id).Max() + 1;
        }
    }
}
