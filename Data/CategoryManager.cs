using System.Data;
using System.Data.SqlClient;

namespace InventoryApp.Data
{
    public class CategoryManager
    {
        readonly SqlConnection con = ConnectionManager.GetConnection();

        // Fetch data from Category
        public DataTable GetCategories()
        {
            con.Open();
            SqlCommand cmd = con.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "SELECT * FROM Category";
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            con.Close();
            return dt;
        }

        // Add new Category
        public void AddCategory(string categoryItem)
        {
            con.Open();
            SqlCommand cmd = con.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "INSERT INTO Category (CategoryItem) VALUES (@categoryitem)";
            cmd.Parameters.AddWithValue("@categoryitem", categoryItem);
            cmd.ExecuteNonQuery();
            con.Close();
        }

        // Update Category
        public void UpdateCategory(int id, string categoryItem)
        {
            con.Open();
            SqlCommand cmd = con.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "UPDATE Category SET CategoryItem = @categoryitem WHERE ID = @id";
            cmd.Parameters.AddWithValue("@categoryitem", categoryItem);
            cmd.Parameters.AddWithValue("@id", id);
            cmd.ExecuteNonQuery();
            con.Close();
        }

        // Delete Category
        public void DeleteCategory(int id)
        {
            con.Open();
            SqlCommand cmd = con.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "DELETE FROM Category WHERE ID = @id";
            cmd.Parameters.AddWithValue("@id", id);
            cmd.ExecuteNonQuery();
            con.Close();
        }
    }
}
