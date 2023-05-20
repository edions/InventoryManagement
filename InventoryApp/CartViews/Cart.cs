using System.Data.SqlClient;
using System.Data;
using System.Windows.Forms;
using System;
using InventoryApp.InventoryApp.dlg;

namespace InventoryApp.InventoryApp.Views
{
    public partial class Cart : Form
    {
        readonly SqlConnection con = ConnectionManager.GetConnection();
        public Cart()
        {
            InitializeComponent();
            DisplayCartItem();
        }

        //FETCH DATA FROM CATEGORY DATABASE
        private void DisplayCartItem()
        {
            using (SqlConnection con = ConnectionManager.GetConnection())
            {
                con.Open();

                using (SqlCommand cmd = new SqlCommand("SELECT * FROM Cart", con))
                {
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dataGridView1.DataSource = dt;
                }

                con.Close();
            }
        }

        //CHECKOUT BUTTON - Cart
        private void button1_Click(object sender, System.EventArgs e)
        {
            using (SqlConnection con = ConnectionManager.GetConnection())
            {
                con.Open();

                // Retrieve the total price from the Cart table
                string query = "SELECT SUM(Price) AS TotalPrice FROM Cart";
                using (SqlCommand command = new SqlCommand(query, con))
                {
                    object result = command.ExecuteScalar();
                    if (result != DBNull.Value)
                    {
                        int totalPrice = Convert.ToInt32(result);

                        // Pass the total price to the Total form and display it
                        Checkout dlg = new Checkout(totalPrice);
                        if (dlg.ShowDialog() == DialogResult.OK)
                        {
                            DisplayCartItem();
                        }
                    }
                }

                con.Close();
            }
        }

        //REMOVE BUTTON - Cart
        private void button2_Click(object sender, System.EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                int id = (int)dataGridView1.SelectedRows[0].Cells["ID"].Value;

                if (MessageBox.Show("Are you sure want to remove this item on your cart?", "Warning!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    con.Open();

                    // Construct the DELETE statement
                    SqlCommand cmd = con.CreateCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "DELETE FROM Cart WHERE ID = @ID";
                    cmd.Parameters.AddWithValue("@ID", id);

                    // Execute the DELETE statement
                    cmd.ExecuteNonQuery();
                    con.Close();

                    // Remove the row from the DataGridView
                    dataGridView1.Rows.RemoveAt(dataGridView1.SelectedRows[0].Index);
                }
            }
            else
            {
                MessageBox.Show("Please select a row to delete.", "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        //TRANSACTION BUTTON - Cart
        private void button3_Click(object sender, EventArgs e)
        {
            Transaction dlg = new Transaction();
            dlg.ShowDialog();
        }
    }
}
