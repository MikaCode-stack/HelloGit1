using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Configuration;

public class DatabaseHandler
{
    private readonly string connectionString;

    public DatabaseHandler()
    {
        connectionString = ConfigurationManager.ConnectionStrings["ShoppingCartDB"].ConnectionString;
    }

    public int CreateCart()
    {
        string query = "INSERT INTO Carts DEFAULT VALUES; SELECT SCOPE_IDENTITY();";
        return ExecuteScalarQuery(query);
    }

    public void AddItem(int cartId, string name, decimal price, int quantity)
    {
        string query = "INSERT INTO Items (CartId, Name, Price, Quantity) VALUES (@CartId, @Name, @Price, @Quantity)";
        var parameters = new Dictionary<string, object>
        {
            { "@CartId", cartId },
            { "@Name", name },
            { "@Price", price },
            { "@Quantity", quantity }
        };
        ExecuteNonQuery(query, parameters);
    }

    public List<Item> GetItems(int cartId)
    {
        string query = "SELECT Name, Price, Quantity FROM Items WHERE CartId = @CartId";
        var parameters = new Dictionary<string, object> { { "@CartId", cartId } };

        List<Item> items = new List<Item>();
        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            conn.Open();
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                foreach (var param in parameters)
                    cmd.Parameters.AddWithValue(param.Key, param.Value);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        items.Add(new Item(reader.GetString(0), reader.GetDecimal(1), reader.GetInt32(2)));
                    }
                }
            }
        }
        return items;
        
    }

    private int ExecuteScalarQuery(string query)
    {
        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            conn.Open();
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }
    }

    private void ExecuteNonQuery(string query, Dictionary<string, object> parameters)
    {
        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            conn.Open();
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                foreach (var param in parameters)
                    cmd.Parameters.AddWithValue(param.Key, param.Value);

                cmd.ExecuteNonQuery();
            }
        }
    }
}
public class Item
{
    public string Name { get; }
    public decimal Price { get; }
    public int Quantity { get; }

    public Item(string name, decimal price, int quantity)
    {
        Name = name;
        Price = price;
        Quantity = quantity;
    }
}
