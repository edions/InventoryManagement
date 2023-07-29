using System;
using System.Data;
using System.Data.SqlClient;

namespace InventoryApp.Data
{
    public class StockManager
    {
        readonly SqlConnection con = ConnectionManager.GetConnection();

        // Get Product by Id
        public int GetProductIdByName(string itemName)
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("SELECT Id FROM Product WHERE name = @itemname", con);
            cmd.Parameters.AddWithValue("@itemname", itemName);
            int productId = Convert.ToInt32(cmd.ExecuteScalar());
            con.Close();
            return productId;
        }

        // Get Stock by Id
        public int GetCurrentStockById(int productId)
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("SELECT stock FROM Product WHERE Id = @productid", con);
            cmd.Parameters.AddWithValue("@productid", productId);
            int currentStock = Convert.ToInt32(cmd.ExecuteScalar());
            con.Close();
            return currentStock;
        }

        // Update Stock
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

        // Insert History
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

        // Get Product stock for Quantity
        public int GetProductStock(int productId)
        {
            int stock = 0;

            con.Open();
            string selectQuery = "SELECT Stock FROM Product WHERE Id = @ProductId";
            using (SqlCommand selectCommand = new SqlCommand(selectQuery, con))
            {
                selectCommand.Parameters.AddWithValue("@ProductId", productId);

                object result = selectCommand.ExecuteScalar();
                if (result != null && result != DBNull.Value)
                {
                    stock = Convert.ToInt32(result);
                }
            }
            con.Close();
            return stock;
        }
    }
}
