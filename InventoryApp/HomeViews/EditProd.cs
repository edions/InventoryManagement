using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace InventoryApp
{
    public partial class EditProd : Form
    {
        readonly SqlConnection con = ConnectionManager.GetConnection();
        readonly private int itemId;
        public EditProd(int id, string name, int price, int stock, int unit, string category)
        {
            InitializeComponent();

            itemId = id;
            textBox1.Text = name;
            textBox2.Text = price.ToString();
            textBox3.Text = stock.ToString();
            textBox4.Text = unit.ToString();
            comboBox1.Text = category;

            //ComboBox Item
            con.Open();
            SqlCommand cmd = new SqlCommand("SELECT CategoryItem FROM Category", con);
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                comboBox1.Items.Add(reader["CategoryItem"].ToString());
            }
            con.Close();
        }

        //UPDATE BUTTON
        private void button1_Click(object sender, EventArgs e)
        {
            con.Open();
            SqlCommand cmd = con.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "UPDATE Product SET name = @name, price = @price, stock = @stock, unit = @unit, category = @category WHERE Id = @id";
            cmd.Parameters.AddWithValue("@name", textBox1.Text);
            cmd.Parameters.AddWithValue("@price", Convert.ToInt32(textBox2.Text));
            cmd.Parameters.AddWithValue("@stock", Convert.ToInt32(textBox3.Text));
            cmd.Parameters.AddWithValue("@unit", Convert.ToInt32(textBox4.Text));
            cmd.Parameters.AddWithValue("@category", comboBox1.Text);
            cmd.Parameters.AddWithValue("@id", itemId);
            cmd.ExecuteNonQuery();

            string selectedItem = comboBox1.Text.Trim();
            if (!string.IsNullOrEmpty(selectedItem) && comboBox1.SelectedIndex == -1)
            {
                SqlCommand command = new SqlCommand("INSERT INTO Category (CategoryItem) VALUES (@categoryitem)", con);
                command.Parameters.AddWithValue("@categoryitem", selectedItem);
                int rowsAffected = command.ExecuteNonQuery();
            }
            con.Close();
            DialogResult = DialogResult.OK;
            Close();
        }

        //CANCEL BUTTON
        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
