using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace InventoryApp
{
    public partial class AddQuantity : Form
    {
        readonly SqlConnection con = ConnectionManager.GetConnection();
        readonly private int itemId;
        public AddQuantity(int id, int quantity)
        {
            InitializeComponent();

            itemId = id;
            textBox2.Text= quantity.ToString();
        }

        //MINUS BUTTON
        private void button3_Click(object sender, EventArgs e)
        {
            if (int.TryParse(textBox2.Text, out int value))
            {
                if (value > 1)
                {
                    value--;
                    textBox2.Text = value.ToString();
                }
            }
        }

        //PLUS BUTTON
        private void button4_Click(object sender, EventArgs e)
        {
            int value = int.Parse(textBox2.Text);
            value++;
            textBox2.Text = value.ToString();
        }

        //SAVE BUTTON
        private void button1_Click(object sender, EventArgs e)
        {
            con.Open();
            SqlCommand cmd = con.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "UPDATE Cart SET Quantity = @quantity WHERE Id = @id";
            cmd.Parameters.AddWithValue("@quantity", textBox2.Text);
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
