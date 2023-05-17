using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace InventoryApp
{
    public partial class EditCat : Form
    {
        readonly SqlConnection con = ConnectionManager.GetConnection();
        readonly private int itemId;
        public EditCat(int id, string categoryitem)
        {
            InitializeComponent();

            itemId = id;
            textBox2.Text= categoryitem.ToString();
        }

        //UPDATE BUTTON
        //Cat
        private void button1_Click(object sender, EventArgs e)
        {
            con.Open();
            SqlCommand cmd = con.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "UPDATE Category SET CategoryItem = @categoryItem WHERE Id = @id";
            cmd.Parameters.AddWithValue("@categoryItem", textBox2.Text);
            cmd.Parameters.AddWithValue("@id", itemId);
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
