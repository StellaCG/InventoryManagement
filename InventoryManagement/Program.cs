using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Library.InventoryManagement.Models;
using Library.InventoryManagement.Services;
using Library.InventoryManagement.Utility;

namespace InventoryManagement
{
    class Program
    {
        static void Main(string[] args)
        {
            // generate an inventory and a cart
            var inventoryService = InventoryService.Current;
            var cartService = CartService.Current;
            if (File.Exists("InventoryData.json")) {
                inventoryService.Load("InventoryData.json");
            }
            if (File.Exists("CartData.json"))
            {
                cartService.Load("CartData.json");
            }
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
                        if (inventoryService.Inventory.Count == 0)
                        {
                            Console.WriteLine("The inventory is empty.");
                            break;
                        } 
                        Console.WriteLine("Would you like to sort the results by:\n1. Name\n2. Unit price");
                        int sort = int.Parse(Console.ReadLine() ?? "0");
                        if (sort < 1 || sort > 2)
                        {
                            Console.WriteLine("Error: invalid selection");
                            continue;
                        }
                        List<Product> sortedResults = inventoryService.SortResults(sort);
                        ViewPages(sortedResults);
                        /*for (int i = 0; i < inventoryService.Inventory.Count; i++)
                        {
                            // Console.WriteLine(inventoryService.Inventory[i].GetType());
                            // Console.WriteLine(inventoryService.Inventory[i].Weight);
                            Console.WriteLine($"{i}. {sortedResults[i].ToString()}");
                        }*/
                    } else if (result == 2)
                    {
                        Console.WriteLine("You chose to edit existing inventory.");
                        Console.WriteLine("Which entry would you like to edit?");
                        var ind = int.Parse(Console.ReadLine() ?? "0");
                        if (ind < 0 || ind > inventoryService.Inventory.Count - 1)
                        {
                            Console.WriteLine("Error: invalid selection");
                            continue;
                        }
                        var selection = inventoryService.Inventory[ind];
                        FillInventory(selection);
                        inventoryService.Update(selection);

                    } else if (result == 3)
                    {
                        Console.WriteLine("You chose to add new inventory.");
                        Console.WriteLine("Will this inventory be measured by (W)eight or (Q)uantity?");
                        string iType = Console.ReadLine();
                        if (iType != "W" && iType != "Q")
                        {
                            Console.WriteLine("Error: invalid selection");
                            continue;
                        }
                        Product? newProduct = null;
                        if (iType == "W")
                        {
                            newProduct = new ProductByWeight();
                        } else
                        {
                            newProduct = new ProductByQuantity();
                        }
                        FillInventory(newProduct);
                        inventoryService.Create(newProduct);

                    } else if (result == 4)
                    {
                        Console.WriteLine("You chose to remove inventory");
                        Console.WriteLine("Which product would you like to delete?");
                        var ind = int.Parse(Console.ReadLine() ?? "0");
                        if (!inventoryService.Inventory.Any(p => p.Id == ind))
                        {
                            Console.WriteLine("Error: invalid selection");
                            return;
                        }
                        inventoryService.Delete(ind);

                    } else if (result == 5)
                    {
                        Console.WriteLine("You chose to view the contents of the cart.");
                        if (cartService.Cart.Count == 0)
                        {
                            Console.WriteLine("The cart is empty.");
                        } else
                        {
                            Console.WriteLine("Would you like to sort by:\n1. Name\n2. Total Price");
                            int sort = int.Parse(Console.ReadLine() ?? "0");
                            if (sort > 0 && sort < 3)
                            {
                                List<Product> sortedResults = cartService.SortResults(sort);
                                ViewPages(sortedResults);
                            } else Console.WriteLine("Error: invalid selection");
                        }

                    } else if (result == 6)
                    {
                        int num = 0;
                        double w = 0;
                        Console.WriteLine("You chose to add an item to the cart.");
                        Console.WriteLine("Which item would you like to add to the cart?");
                        var ind = int.Parse(Console.ReadLine() ?? "0");
                        if (!inventoryService.Inventory.Any(p => p.Id == ind))
                        {
                            Console.WriteLine("Error: invalid selection");
                            continue;
                        }
                        Product? selectionCopy = null;
                        var selection = inventoryService.Inventory.Find(p => p.Id == ind);
                        if (selection is ProductByWeight)
                        {
                            Console.WriteLine("How much of this item do you want?");
                            w = double.Parse(Console.ReadLine() ?? "0");
                            selectionCopy = new ProductByWeight(selection as ProductByWeight);
                            var sc = selectionCopy as ProductByWeight;
                            sc.Weight = w;
                        } else
                        {
                            Console.WriteLine("How many of this item do you want?");
                            num = int.Parse(Console.ReadLine() ?? "0");
                            selectionCopy = new Product(selection);
                        }

                        if (selectionCopy is ProductByWeight)
                        {
                            var sbw = selection as ProductByWeight;
                            selectionCopy.Weight = w;
                            if (sbw.Weight >= w) sbw.Weight -= w;
                            else
                            {
                                Console.WriteLine("Not enough in stock.");
                                continue;
                            }
                        } else
                        {
                            selectionCopy.Quantity = num;
                            if (selection.Quantity >= num) selection.Quantity -= num;
                            else
                            {
                                Console.WriteLine("Not enough in stock");
                                continue;
                            }
                        }
                        cartService.Cart.Add(selectionCopy);

                    } else if (result == 7)
                    {
                        Console.WriteLine("You chose to remove an item from the cart");
                        Console.WriteLine("Which item would you like to remove?");
                        var ind = int.Parse(Console.ReadLine() ?? "0");
                        Console.WriteLine("How many would you like to remove?");
                        var num = int.Parse(Console.ReadLine() ?? "0");
                        while (num > cartService.Cart[ind].Quantity || num < 0)
                        {
                            Console.WriteLine("Please enter a valid quantity");
                            num = int.Parse(Console.ReadLine() ?? "0");
                        }
                        foreach (Product p in inventoryService.Inventory)
                        {
                            if (p.Name == cartService.Cart[ind].Name)
                            {
                                p.Quantity += num;
                                break;
                            }
                        }
                        cartService.Delete(ind, num);
                    } else if (result == 8)
                    {
                        Console.WriteLine("You chose to checkout.");
                        Console.WriteLine("Here is your cart:");
                        for (int i = 0; i < cartService.Cart.Count; i++)
                        {
                            Console.WriteLine($"{i}. {cartService.Cart[i].ToString()}");
                        }
                        double total = cartService.CalculatePrice();
                        Console.WriteLine(String.Format("Your total is {0:0.00}.", total));
                        Console.WriteLine("Please enter your payment username:");
                        _ = Console.ReadLine();
                        Console.WriteLine("Please enter your payment password:");
                        _ = Console.ReadLine();
                        Console.WriteLine("Payment successful.");
                        File.Delete("CartData.json");
                        Console.WriteLine("Thank you for shopping!");
                        Environment.Exit(0);
                    } else if (result == 9)
                    {
                        Console.WriteLine("You chose to search for a product.");
                        Console.WriteLine("Enter your search term:");
                        string search = Console.ReadLine() ?? string.Empty;
                        Console.WriteLine("Search results:");
                        List<Product> searchResults = new List<Product>();
                        foreach (Product p in inventoryService.Inventory)
                        {
                            if (p.Name.Contains(search.ToLower()) || p.Description.Contains(search.ToLower()))
                            {
                                searchResults.Add(p);
                                // Console.WriteLine($"{inventoryService.Inventory.IndexOf(p)}. {p.Name}: {p.Description}");
                            }
                        }
                        ViewPages(searchResults);
                    } else if (result == 10)
                    {
                        Console.WriteLine("You chose to save the inventory and cart.");
                        inventoryService.Save("InventoryData.json");
                        cartService.Save("CartData.json");
                    } else if (result == 11)
                    {
                        break;
                    }
                    input = Console.ReadLine();
                }
            }
            Console.WriteLine("Thank you for shopping!");
        }

        public static void ViewPages(List<Product> list)
        {
            var nav = new ListNavigator<Product>(list);
            while (true)
            {
                foreach (var d in nav.GetCurrentPage())
                {
                    Console.WriteLine(d.Value);
                }
                if (nav.HasNextPage && nav.HasPreviousPage)
                {
                    Console.WriteLine("Would you like to go to the [N]ext page, [P]revious page, or the [M]enu?");
                    string inp = Console.ReadLine();
                    if (inp == "N") nav.GoForward();
                    else if (inp == "P") nav.GoBackward();
                    else if (inp == "M") break;
                    else Console.WriteLine("Invalid input");
                }
                else if (nav.HasNextPage)
                {
                    Console.WriteLine("Would you like to go to the [N]ext page or the [M]enu?");
                    string inp = Console.ReadLine();
                    if (inp == "N") nav.GoForward();
                    else if (inp == "M") break;
                    else Console.WriteLine("Invalid input");
                }
                else if (nav.HasPreviousPage)
                {
                    Console.WriteLine("Would you like to go to the [P]revious page or the [M]enu?");
                    string inp = Console.ReadLine();
                    if (inp == "P") nav.GoBackward();
                    else if (inp == "M") break;
                    else Console.WriteLine("Invalid input.");
                }
                else break;
            }
        }
        public static void FillInventory(Product? product)
        {
            if (product == null)
            {
                return;
            }
            Console.WriteLine("What is the name of the product?");
            product.Name = Console.ReadLine() ?? string.Empty;
            Console.WriteLine("What is the description of the product?");
            product.Description = Console.ReadLine() ?? string.Empty;
            if (product is ProductByWeight)
            {
                var pbw = product as ProductByWeight;
                Console.WriteLine("What is the weight of the product?");
                pbw.Weight = double.Parse(Console.ReadLine() ?? "0");
            }
            else
            {
                Console.WriteLine("What is the quantity of the product?");
                product.Quantity = int.Parse(Console.ReadLine() ?? "0");
                Console.WriteLine("Is this product BOGO? (Y/N)");
                string sale = Console.ReadLine() ?? null;
                while (sale != "Y" && sale != "N" && sale != null)
                {
                    Console.WriteLine("Invalid input.");
                    Console.WriteLine("Is this product BOGO? (Y/N)");
                    sale = Console.ReadLine() ?? null;
                }
                if (sale == "Y") product.Bogo = true;
                else if (sale == "N" || sale == null) product.Bogo = false;
            }
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
            Console.WriteLine("10. Save Inventory or Cart.");
            Console.WriteLine("11. Exit");
        }
    }
}

