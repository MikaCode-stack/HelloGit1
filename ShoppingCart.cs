using System;
using System.Collections.Generic;
using System.Globalization;

class ShoppingCartSystem
{
    private DatabaseHandler dbHandler;
    private int cartId;

    public ShoppingCartSystem()
    {
        dbHandler = new DatabaseHandler();
    }

    public void Run()
    {
        Console.WriteLine("Welcome to the Shopping Cart System!");
        Console.WriteLine("-------------------------------------");

        while (true)
        {
            cartId = dbHandler.CreateCart(); // ✅ Create a new cart in the database
            Console.WriteLine($"New Cart Created with ID: {cartId}");

            AddItemsToCart();
            DisplayCartSummary();

            Console.Write("\nWould you like to start a new cart? (Type 'Exit' to exit, any key to continue): ");
            string response = Console.ReadLine()?.Trim().ToLower();
            if (response == "exit")
            {
                Console.WriteLine("Thank you for using the Shopping Cart System. Goodbye!");
                break;
            }
        }
    }

    private void AddItemsToCart()
    {
        while (true)
        {
            Console.WriteLine("Enter item name (or type 'done' to finish): ");
            string name = Console.ReadLine()?.Trim();
            if (string.IsNullOrWhiteSpace(name))
            {
                Console.WriteLine("Invalid input. Please enter a valid name.");
                continue;
            }
            if (name.ToLower() == "done") break;

            decimal price = GetValidDecimal("Enter the price of the item: ");
            int quantity = GetValidInt("Enter the quantity of the item: ");

            dbHandler.AddItem(cartId, name, price, quantity); // ✅ Store item in database
            Console.WriteLine("Item added successfully!");
        }
    }

    private void DisplayCartSummary()
    {
        List<Item> cartItems = dbHandler.GetItems(cartId); // ✅ Fetch stored items from database

        decimal totalCost = CalculateTotalCost(cartItems);
        decimal discount = CalculateDiscount(totalCost);
        decimal finalCost = totalCost - discount;

        Console.WriteLine("\n--- Cart Items Summary ---");
        foreach (var item in cartItems)
        {
            Console.WriteLine($"{item.Name} - ${item.Price} x {item.Quantity} = ${item.Price * item.Quantity}");
        }
        Console.WriteLine($"Total Cost: ${totalCost:F2}");
        Console.WriteLine($"Discount Applied: ${discount:F2}");
        Console.WriteLine($"Final Cost: ${finalCost:F2}");
    }

    private decimal GetValidDecimal(string prompt)
    {
        while (true)
        {
            Console.WriteLine(prompt);
            if (decimal.TryParse(Console.ReadLine(), NumberStyles.Currency, CultureInfo.InvariantCulture, out decimal value))
            {
                return value;
            }
            Console.WriteLine("Invalid input. Please enter a valid number.");
        }
    }

    private int GetValidInt(string prompt)
    {
        while (true)
        {
            Console.WriteLine(prompt);
            if (int.TryParse(Console.ReadLine(), out int value) && value > 0)
            {
                return value;
            }
            Console.WriteLine("Invalid input. Please enter a valid quantity (greater than 0).");
        }
    }

    private decimal CalculateTotalCost(List<Item> cart)
    {
        decimal total = 0;
        foreach (var item in cart)
        {
            total += item.Price * item.Quantity;
        }
        return total;
    }

    private decimal CalculateDiscount(decimal totalCost)
    {
        if (totalCost > 20) return totalCost * 0.2m;
        if (totalCost > 10) return totalCost * 0.1m;
        return 0.00m;
    }
}
