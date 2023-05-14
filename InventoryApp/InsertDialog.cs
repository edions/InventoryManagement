using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace InventoryApp
{
    public partial class InsertDialog : Form
    {
        readonly SqlConnection con = ConnectionManager.GetConnection();
        public InsertDialog()
        {
            InitializeComponent();

            comboBox1.Items.Add("ITEM1");
            comboBox1.Items.Add("ITEM2");
            comboBox1.Items.Add("ITEM3");
            comboBox1.Items.Add("ITEM4");
        }

        //SAVE BUTTON
        private void button1_Click(object sender, EventArgs e)
        {
            con.Open();
            SqlCommand cmd = con.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "INSERT INTO Inventory (name, price, stock, unit, category) VALUES (@name, @price, @stock, @unit, @category)";
            cmd.Parameters.AddWithValue("@name", textBox1.Text);
            cmd.Parameters.AddWithValue("@price", Convert.ToInt32(textBox2.Text));
            cmd.Parameters.AddWithValue("@stock", Convert.ToInt32(textBox3.Text));
            cmd.Parameters.AddWithValue("@unit", Convert.ToInt32(textBox4.Text));
            cmd.Parameters.AddWithValue("@category", comboBox1.Text);
            cmd.ExecuteNonQuery();
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
