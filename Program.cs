using System;
using System.Collections.Generic;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Welcome to the Shopping Cart Calculator!");
        Console.WriteLine("----------------------------------------");

        // List of items in the cart
        List<Item> cart = new List<Item>();

        // Add items to the cart
        cart.Add(new Item("Apple", 0.50, 3));
        cart.Add(new Item("Bread", 2.00, 1));
        cart.Add(new Item("Milk", 1.50, 2));
        cart.Add(new Item("Eggs", 1.20, 0));

        // Calculate total cost
        double totalCost = CalculateTotalCost(cart);

        // Apply discount
        double discount = CalculateDiscount(totalCost);
        double finalCost = totalCost - discount;

        // Display results
        Console.WriteLine($"Total Cost: ${totalCost}");
        Console.WriteLine($"Discount Applied: ${discount}"); // Bug 1 - Syntax Error
        Console.WriteLine($"Final Cost: ${finalCost}");
    }

    static double CalculateTotalCost(List<Item> cart)
    {
        double total = 0;

        foreach (var item in cart)
        {
            total += item.Price * item.Quantity;
        }

        return total;
    }

    // Logic issue. If Cost > 20 then it is already > 10. No further discount applied
    /*static double CalculateDiscount(double totalCost)
    {
        double discount = 0.00;

        if (totalCost > 10)
        {
            discount = totalCost * 0.1;
        }
        else if (totalCost > 20)
        {
            discount = totalCost * 0.2;
        }

        return discount;
    }
}*/

static double CalculateDiscount(double totalCost)
    {
        double discount = 0.00;

        if (totalCost > 20)
        {
            discount = totalCost * 0.1;
        }
        else if (totalCost > 10)
        {
            discount = totalCost * 0.2;
        }

        return discount;
    }
}

class Item
{
    public string Name { get; set; }
    public double Price { get; set; }
    public int Quantity { get; set; }

    public Item(string name, double price, int quantity)
    {
        Name = name;
        Price = price;
        Quantity = quantity;
    }
}