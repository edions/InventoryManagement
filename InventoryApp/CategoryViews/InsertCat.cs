using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace InventoryApp
{
    public partial class InsertCat : Form
    {
        readonly SqlConnection con = ConnectionManager.GetConnection();
        public InsertCat()
        {
            InitializeComponent();
        }

        //INSERT BUTTON
        //Cat
        private void button1_Click(object sender, EventArgs e)
        {
            con.Open();
            SqlCommand cmd = con.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "INSERT INTO Category (CategoryItem) VALUES (@categoryItem)";
            cmd.Parameters.AddWithValue("@categoryItem", textBox2.Text);
            cmd.ExecuteNonQuery();
            con.Close();
            DialogResult = DialogResult.OK;
            Close();
        }

        //CANCEL BUTTON
        //Cat
        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
