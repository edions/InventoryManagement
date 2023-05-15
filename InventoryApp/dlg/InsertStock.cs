using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace InventoryApp
{
    public partial class InsertStock : Form
    {
        readonly SqlConnection con = ConnectionManager.GetConnection();
        readonly private string itemName;
        public InsertStock(string name)
        {
            InitializeComponent();
            itemName = name;
            label3.Text = name;
        }

        //INSERT STOCK BUTTON
        private void button1_Click(object sender, EventArgs e)
        {
            con.Open();
            SqlCommand cmd = con.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "SELECT Id, stock FROM Product WHERE name = @itemname";
            cmd.Parameters.AddWithValue("@itemname", itemName);

            SqlDataReader reader = cmd.ExecuteReader();
            int currentStock = 0;
            int productId = 0;
            if (reader.Read())
            {
                currentStock = reader.GetInt32(1);
                productId = reader.GetInt32(0);
            }
            reader.Close();

            cmd.CommandText = "UPDATE Product SET stock = @stock WHERE name = @itemname";
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@stock", currentStock + Convert.ToInt32(textBox2.Text));
            cmd.Parameters.AddWithValue("@itemname", itemName);
            cmd.ExecuteNonQuery();

            cmd.CommandText = "INSERT INTO History (ProductID, [Added Stocks], [Date]) VALUES (@productId, @addedStocks, GETDATE())";
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@productId", productId);
            cmd.Parameters.AddWithValue("@addedStocks", Convert.ToInt32(textBox2.Text));
            cmd.ExecuteNonQuery();

            con.Close();
            DialogResult = DialogResult.OK;
        }

        //CANCEL BUTTON
        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
