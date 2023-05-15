using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace InventoryApp
{
    public partial class Insert : Form
    {
        readonly SqlConnection con = ConnectionManager.GetConnection();
        public Insert()
        {
            InitializeComponent();

            //ComboBox Item
            con.Open();
            SqlCommand command = new SqlCommand("SELECT CategoryItem FROM Category", con);
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                comboBox1.Items.Add(reader["CategoryItem"].ToString());
            }
            con.Close();
        }

        //SAVE BUTTON
        private void button1_Click(object sender, EventArgs e)
        {
            con.Open();
            SqlCommand cmd = con.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "INSERT INTO Product (name, price, stock, unit, category) VALUES (@name, @price, @stock, @unit, @category)";
            cmd.Parameters.AddWithValue("@name", textBox1.Text);
            cmd.Parameters.AddWithValue("@price", Convert.ToInt32(textBox2.Text));
            cmd.Parameters.AddWithValue("@stock", Convert.ToInt32(textBox3.Text));
            cmd.Parameters.AddWithValue("@unit", Convert.ToInt32(textBox4.Text));
            cmd.Parameters.AddWithValue("@category", comboBox1.Text);
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
