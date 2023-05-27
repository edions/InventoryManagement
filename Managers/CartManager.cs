using System.Data.SqlClient;
using System.Data;
using System;

namespace InventoryApp.Entity
{
    public class CartManager
    {
        private readonly SqlConnection con;

        public CartManager()
        {
            con = ConnectionManager.GetConnection();
        }

        public void UpdateQuantityInCart(int itemId, string quantity)
        {
            con.Open();
            using (SqlCommand cmd = con.CreateCommand())
            {
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "UPDATE Cart SET Quantity = @quantity WHERE Id = @id";
                cmd.Parameters.AddWithValue("@quantity", quantity);
                cmd.Parameters.AddWithValue("@id", itemId);
                cmd.ExecuteNonQuery();
            }
            con.Close();
        }

        public DataTable GetCartItems()
        {
            using (SqlConnection con = ConnectionManager.GetConnection())
            {
                con.Open();

                using (SqlCommand cmd = new SqlCommand("SELECT * FROM Cart", con))
                {
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    return dt;
                }
            }
        }

        public decimal GetTotalPrice()
        {
            decimal totalPrice = 0;

            using (SqlConnection con = ConnectionManager.GetConnection())
            {
                con.Open();

                string query = "SELECT SUM(Price * Quantity) AS TotalPrice FROM Cart";
                using (SqlCommand command = new SqlCommand(query, con))
                {
                    object result = command.ExecuteScalar();
                    if (result != DBNull.Value && result != null)
                    {
                        totalPrice = Convert.ToDecimal(result);
                    }
                }
            }

            return totalPrice;
        }

        public void RemoveCartItem(int id)
        {
            using (SqlConnection con = ConnectionManager.GetConnection())
            {
                con.Open();

                SqlCommand cmd = con.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "DELETE FROM Cart WHERE ID = @ID";
                cmd.Parameters.AddWithValue("@ID", id);
                cmd.ExecuteNonQuery();
            }
        }
    }
}
