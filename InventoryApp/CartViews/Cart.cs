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

                // Retrieve the total price based on the quantity from the Cart table
                string query = "SELECT SUM(Price * Quantity) AS TotalPrice FROM Cart";
                using (SqlCommand command = new SqlCommand(query, con))
                {
                    object result = command.ExecuteScalar();
                    if (result != DBNull.Value && result != null)
                    {
                        int totalPrice = Convert.ToInt32(result);

                        // Pass the total price to the Total form and display it
                        Checkout dlg = new Checkout(totalPrice);
                        if (dlg.ShowDialog() == DialogResult.OK)
                        {
                            DisplayCartItem();
                        }
                    }
                    else
                    {
                        // Cart table is empty, handle the scenario here
                        MessageBox.Show("Cart is empty.", "Empty Cart", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }

                con.Close();
            }
        }

        //ADD QUANTITY BYTTON - Cart
        private void button4_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                // Get the data from the selected row
                DataGridViewRow row = dataGridView1.SelectedRows[0];
                int id = (int)row.Cells["Id"].Value;
                int quantity = (int)row.Cells["Quantity"].Value;

                // Pass the data to EditCat
                Quantity dlg = new Quantity(id, quantity);
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    // Refresh DataGridView when "EditCategory Dialog" is closed
                    DisplayCartItem();
                }
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
                MessageBox.Show("Cart is empty.", "Empty Cart", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
