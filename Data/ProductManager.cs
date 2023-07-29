using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace InventoryApp.Data
{
    public class ProductManager
    {
        readonly SqlConnection con = ConnectionManager.GetConnection();

        // Fetch data from Product
        public DataTable GetProducts()
        {
            con.Open();
            SqlCommand cmd = con.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "SELECT * FROM Product";
            cmd.ExecuteNonQuery();
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            con.Close();
            return dt;
        }

        // Search Product
        public DataTable SearchProducts(string searchTerm)
        {
            con.Open();
            DataTable dt = new DataTable("Customer");

            using (SqlCommand cmd = new SqlCommand("SELECT * FROM Product WHERE Name LIKE '%' + @SearchTerm + '%' OR Category LIKE '%' + @SearchTerm + '%'", con))
            {
                cmd.Parameters.AddWithValue("@SearchTerm", "%" + searchTerm + "%");
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(dt);
            }

            con.Close();
            return dt;
        }

        // Fetch data from Category for ComboBox
        public string[] GetCategoryItems()
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("SELECT CategoryItem FROM Category", con);
            SqlDataReader reader = cmd.ExecuteReader();
            var categoryItems = new List<string>();
            while (reader.Read())
            {
                categoryItems.Add(reader["CategoryItem"].ToString());
            }
            con.Close();

            return categoryItems.ToArray();
        }

        // Add new Prodcut
        public void InsertProduct(string name, int price, int stock, int unit, string category)
        {
            con.Open();
            SqlCommand cmd = con.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "INSERT INTO Product (name, price, stock, unit, category) VALUES (@name, @price, @stock, @unit, @category)";
            cmd.Parameters.AddWithValue("@name", name);
            cmd.Parameters.AddWithValue("@price", price);
            cmd.Parameters.AddWithValue("@stock", stock);
            cmd.Parameters.AddWithValue("@unit", unit);
            cmd.Parameters.AddWithValue("@category", category);
            cmd.ExecuteNonQuery();
            con.Close();
        }

        // Update Product
        public void UpdateProduct(int id, string name, int price, int stock, int unit, string category)
        {
            con.Open();
            SqlCommand cmd = con.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "UPDATE Product SET name = @name, price = @price, stock = @stock, unit = @unit, category = @category WHERE Id = @id";
            cmd.Parameters.AddWithValue("@name", name);
            cmd.Parameters.AddWithValue("@price", price);
            cmd.Parameters.AddWithValue("@stock", stock);
            cmd.Parameters.AddWithValue("@unit", unit);
            cmd.Parameters.AddWithValue("@category", category);
            cmd.Parameters.AddWithValue("@id", id);
            cmd.ExecuteNonQuery();
            con.Close();
        }

        // Delete Product
        public void DeleteProduct(int id)
        {
            con.Open();
            SqlCommand cmd = con.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "DELETE FROM Product WHERE ID = @ID";
            cmd.Parameters.AddWithValue("@ID", id);
            cmd.ExecuteNonQuery();
            con.Close();
        }

        // Add new Category
        public void InsertCategory(string categoryItem)
        {
            con.Open();
            SqlCommand command = new SqlCommand("INSERT INTO Category (CategoryItem) VALUES (@categoryitem)", con);
            command.Parameters.AddWithValue("@categoryitem", categoryItem);
            command.ExecuteNonQuery();
            con.Close();
        }

        // Add item to the cart
        public static bool AddItemToCart(string name, int price)
        {
            using (SqlConnection con = ConnectionManager.GetConnection())
            {
                con.Open();
                string selectQuery = "SELECT Quantity, Price FROM Cart WHERE Name = @Name";

                using (SqlCommand selectCommand = new SqlCommand(selectQuery, con))
                {
                    selectCommand.Parameters.AddWithValue("@Name", name);
                    SqlDataReader reader = selectCommand.ExecuteReader();

                    if (reader.Read())
                    {
                        // Item already exists in the cart, update the quantity and price
                        int existingQuantity = Convert.ToInt32(reader["Quantity"]);
                        int existingPrice = Convert.ToInt32(reader["Price"]);
                        int newQuantity = existingQuantity + 1;
                        int newPrice = existingPrice;

                        reader.Close();

                        string updateQuery = "UPDATE Cart SET Quantity = @Quantity, Price = @Price WHERE Name = @Name";

                        using (SqlCommand updateCommand = new SqlCommand(updateQuery, con))
                        {
                            updateCommand.Parameters.AddWithValue("@Quantity", newQuantity);
                            updateCommand.Parameters.AddWithValue("@Price", newPrice);
                            updateCommand.Parameters.AddWithValue("@Name", name);
                            updateCommand.ExecuteNonQuery();
                        }
                    }
                    else
                    {
                        reader.Close();

                        // Item does not exist in the cart, insert a new row
                        string insertQuery = "INSERT INTO Cart (ProductId, Uid, Name, Price, Quantity) " +
                                             "VALUES ((SELECT Id FROM Product WHERE Name = @Name), @Uid, @Name, @Price, 1)";

                        int currentUID = UserSession.SessionUID;

                        using (SqlCommand insertCommand = new SqlCommand(insertQuery, con))
                        {
                            insertCommand.Parameters.AddWithValue("@Name", name);
                            insertCommand.Parameters.AddWithValue("@Price", price);
                            insertCommand.Parameters.AddWithValue("@Uid", currentUID);
                            insertCommand.ExecuteNonQuery();
                        }
                    }
                }

                con.Close();
            }

            return true;
        }
    }
}
