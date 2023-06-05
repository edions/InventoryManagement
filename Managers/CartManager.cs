using System.Data.SqlClient;
using System.Data;
using System;
using System.Windows.Forms;

namespace InventoryApp.Entity
{
    public class CartManager
    {
        readonly SqlConnection con = ConnectionManager.GetConnection();

        // Fetch data from Cart
        public DataTable GetCartItems()
        {
            using (SqlConnection con = ConnectionManager.GetConnection())
            {
                con.Open();

                using (SqlCommand cmd = new SqlCommand("SELECT Name, Price, Quantity, ProductId FROM [Cart]", con))
                {
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    return dt;
                }
            }
        }

        // Update Quantity
        public void UpdateQuantityInCart(int itemId, string quantity)
        {
            con.Open();
            using (SqlCommand cmd = con.CreateCommand())
            {
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "UPDATE Cart SET Quantity = @quantity WHERE ProductId = @productId";
                cmd.Parameters.AddWithValue("@quantity", quantity);
                cmd.Parameters.AddWithValue("@productId", itemId);
                cmd.ExecuteNonQuery();
            }
            con.Close();
        }

        // Total Price
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

        // Remove product from Cart
        public void RemoveCartItem(int productId)
        {
            using (SqlConnection con = ConnectionManager.GetConnection())
            {
                con.Open();

                SqlCommand cmd = con.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "DELETE FROM Cart WHERE ProductId = @ProductId";
                cmd.Parameters.AddWithValue("@ProductId", productId);
                cmd.ExecuteNonQuery();
            }
        }

        // Load Cart items to ListBox
        public void LoadCartItems(ListBox listBox)
        {
            try
            {
                con.Open();

                string selectQuery = "SELECT Name, Price, Quantity FROM Cart";

                using (SqlCommand command = new SqlCommand(selectQuery, con))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        listBox.Items.Clear();

                        while (reader.Read())
                        {
                            string name = reader["Name"].ToString();
                            int price = Convert.ToInt32(reader["Price"]);
                            int quantity = Convert.ToInt32(reader["Quantity"]);

                            string item = $"{quantity} x {name} - ${price}";
                            listBox.Items.Add(item);
                        }
                    }
                }

                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while loading cart items: " + ex.Message);
            }
        }
    }
}
