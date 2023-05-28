using System;
using System.Data;
using System.Data.SqlClient;

namespace InventoryApp.Entity
{
    public class StockManager
    {
        readonly SqlConnection con = ConnectionManager.GetConnection();

        public int GetProductIdByName(string itemName)
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("SELECT Id FROM Product WHERE name = @itemname", con);
            cmd.Parameters.AddWithValue("@itemname", itemName);
            int productId = Convert.ToInt32(cmd.ExecuteScalar());
            con.Close();
            return productId;
        }

        public int GetCurrentStockById(int productId)
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("SELECT stock FROM Product WHERE Id = @productid", con);
            cmd.Parameters.AddWithValue("@productid", productId);
            int currentStock = Convert.ToInt32(cmd.ExecuteScalar());
            con.Close();
            return currentStock;
        }

        public void UpdateStock(int productId, int newStock)
        {
            con.Open();
            SqlCommand cmd = con.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "UPDATE Product SET stock = @stock WHERE Id = @productid";
            cmd.Parameters.AddWithValue("@stock", newStock);
            cmd.Parameters.AddWithValue("@productid", productId);
            cmd.ExecuteNonQuery();
            con.Close();
        }

        public void InsertHistory(int productId, int addedStocks)
        {
            con.Open();
            SqlCommand cmd = con.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "INSERT INTO History (ProductID, [Added Stocks], [Date]) VALUES (@productId, @addedStocks, GETDATE())";
            cmd.Parameters.AddWithValue("@productId", productId);
            cmd.Parameters.AddWithValue("@addedStocks", addedStocks);
            cmd.ExecuteNonQuery();
            con.Close();
        }
    }
}
