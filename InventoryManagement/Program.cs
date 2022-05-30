using System;
using System.Collections.Generic;
using Library.InventoryManagement.Models;
using Library.InventoryManagement.Services;

namespace InventoryManagement
{
    class Program
    {
        static void Main(string[] args)
        {
            // generate an inventory and a cart
            var inventoryService = new InventoryService();
            var cartService = new CartService();
            // menu
            PrintMenu();
            var input = Console.ReadLine();
            bool cont = true;
            while(cont)
            {
                if (int.TryParse(input, out int result))
                {
                    if (result == 1)
                    {
                        Console.WriteLine("You chose to view inventory.");
                        for (int i = 0; i < inventoryService.Inventory.Count; i++)
                        {
                            Console.WriteLine($"{i}. {inventoryService.Inventory[i].ToString()}");
                        }
                    } else if (result == 2)
                    {
                        Console.WriteLine("You chose to edit existing inventory.");
                        Console.WriteLine("Which entry would you like to edit?");
                        var ind = int.Parse(Console.ReadLine() ?? "0");
                        var selection = inventoryService.Inventory[ind];
                        FillInventory(selection);
                        inventoryService.Update(selection);

                    } else if (result == 3)
                    {
                        Console.WriteLine("You chose to add new inventory.");
                        var newProduct = new Product();
                        FillInventory(newProduct);
                        inventoryService.Create(newProduct);

                    } else if (result == 4)
                    {
                        Console.WriteLine("You chose to remove inventory");
                        Console.WriteLine("Which product would you like to delete?");
                        var ind = int.Parse(Console.ReadLine() ?? "0");
                        inventoryService.Delete(ind);

                    } else if (result == 5)
                    {
                        Console.WriteLine("You chose to view the contents of the cart.");
                        for (int i = 0; i < cartService.Cart.Count; i++)
                        {
                            Console.WriteLine($"{i}. {cartService.Cart[i].ToString()}");
                        }

                    } else if (result == 6)
                    {
                        Console.WriteLine("You chose to add an item to the cart.");
                        Console.WriteLine("Which item would you like to add to the cart?");
                        var ind = int.Parse(Console.ReadLine() ?? "0");
                        Console.WriteLine("How many of this item do you want?");
                        int num = int.Parse(Console.ReadLine() ?? "0");
                        var selection = inventoryService.Inventory[ind];
                        Product selectionCopy = new Product(selection);
                        selectionCopy.Quantity = num;
                        cartService.Cart.Add(selectionCopy);
                        selection.Quantity -= num;

                    } else if (result == 7)
                    {
                        Console.WriteLine("You chose to remove an item from the cart");
                        Console.WriteLine("Which item would you like to remove?");
                        var ind = int.Parse(Console.ReadLine() ?? "0");
                        Console.WriteLine("How many would you like to remove?");
                        var num = int.Parse(Console.ReadLine() ?? "0");
                        cartService.Delete(ind, num);
                    } else if (result == 8)
                    {
                        Console.WriteLine("You chose to checkout.");
                        Console.WriteLine("Here is your cart:");
                        for (int i = 0; i < cartService.Cart.Count; i++)
                        {
                            Console.WriteLine($"{i}. {cartService.Cart[i].ToString()}");
                        }
                        double total = 0;
                        foreach (Product p in cartService.Cart)
                        {
                            total += (p.Quantity * p.Price);
                        }
                        double tax = 0.07 * total;
                        Console.WriteLine($"Your total is {total + tax}.");
                        Console.WriteLine("Thank you for shopping!");
                        Environment.Exit(0);
                    } else if (result == 9)
                    {
                        Console.WriteLine("You chose to search for a product.");
                        Console.WriteLine("Enter your search term:");
                        string search = Console.ReadLine() ?? string.Empty;
                        Console.WriteLine("Search results:");
                        foreach (Product p in inventoryService.Inventory)
                        {
                            if (p.Name.Contains(search.ToLower()) || p.Description.Contains(search.ToLower()))
                            {
                                Console.WriteLine($"{inventoryService.Inventory.IndexOf(p)}. {p.Name}: {p.Description}");
                            }
                        }
                    } else if (result == 10)
                    {
                        break;
                    }
                    input = Console.ReadLine();
                }
            }
            Console.WriteLine("Thank you for shopping!");
        }

        public static void FillInventory(Product product)
        {
            Console.WriteLine("What is the name of the product?");
            product.Name = Console.ReadLine() ?? string.Empty;
            Console.WriteLine("What is the description of the product?");
            product.Description = Console.ReadLine() ?? string.Empty;
            Console.WriteLine("What is the quantity of the product?");
            product.Quantity = int.Parse(Console.ReadLine() ?? "0");
            Console.WriteLine("What is the price of the product?");
            product.Price = double.Parse(Console.ReadLine() ?? "0");
        }
        public static void PrintMenu()
        {
            Console.WriteLine("Inventory Management App Menu");
            Console.WriteLine("Select an option:");
            Console.WriteLine("1. View Inventory");
            Console.WriteLine("2. Edit Existing Inventory");
            Console.WriteLine("3. Add New Inventory");
            Console.WriteLine("4. Remove Inventory");
            Console.WriteLine("5. View Contents of Cart");
            Console.WriteLine("6. Add Item to Cart");
            Console.WriteLine("7. Remove Item from Cart");
            Console.WriteLine("8. Checkout");
            Console.WriteLine("9. Search for an Item");
            Console.WriteLine("10. Exit");
        }
    }
}

