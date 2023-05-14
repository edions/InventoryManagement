using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace InventoryApp
{
    public partial class EditDialog : Form
    {
        readonly SqlConnection con = ConnectionManager.GetConnection();
        readonly private int itemId;
        public EditDialog(int id, string name, int price, int stock, int unit, string category)
        {
            InitializeComponent();

            itemId = id;
            textBox1.Text = name;
            textBox2.Text = price.ToString();
            textBox3.Text = stock.ToString();
            textBox4.Text = unit.ToString();
            comboBox1.Text = category;

            //ComboBox Item
            comboBox1.Items.Add("ITEM1");
            comboBox1.Items.Add("ITEM2");
            comboBox1.Items.Add("ITEM3");
            comboBox1.Items.Add("ITEM4");
        }

        //UPDATE BUTTON
        private void button1_Click(object sender, EventArgs e)
        {
            con.Open();
            SqlCommand cmd = con.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "UPDATE Inventory SET name = @name, price = @price, stock = @stock, unit = @unit, category = @category WHERE Id = @id";
            cmd.Parameters.AddWithValue("@name", textBox1.Text);
            cmd.Parameters.AddWithValue("@price", Convert.ToInt32(textBox2.Text));
            cmd.Parameters.AddWithValue("@stock", Convert.ToInt32(textBox3.Text));
            cmd.Parameters.AddWithValue("@unit", Convert.ToInt32(textBox4.Text));
            cmd.Parameters.AddWithValue("@category", comboBox1.Text);
            cmd.Parameters.AddWithValue("@id", itemId);
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
