using System;
using System.Data;
using System.Windows.Forms;
using System.Data.SqlClient;
using InventoryApp.InventoryApp.dlg;

namespace InventoryApp
{
    public partial class Home : Form
    {
        readonly SqlConnection con = ConnectionManager.GetConnection();
        public Home()
        {
            InitializeComponent();
            DisplayData();
        }

        //FETCH DATA FROM PRODUCT DATABASE
        public void DisplayData()
        {
            con.Open();
            SqlCommand cmd = con.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "select * from Product";
            cmd.ExecuteNonQuery();
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            dataGridView1.DataSource = dt;
            con.Close();
        }

        //SEARCH AND DISPLAY RESULTS
        private void PerformSearch()
        {
            con.Open();
            DataTable dt = new DataTable("Customer");

            using (SqlCommand cmd = new SqlCommand("SELECT * FROM Product WHERE Name LIKE '%' + @SearchTerm + '%' OR Category LIKE '%' + @SearchTerm + '%'", con))
            {
                cmd.Parameters.AddWithValue("@SearchTerm", "%" + textBox1.Text + "%");
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(dt);
                dataGridView1.DataSource = dt;
            }

            con.Close();
        }

        //SEARCH BUTTON
        private void button6_Click(object sender, EventArgs e)
        {
            PerformSearch();
        }

        //IF USER PRESS ENTER KEY
        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                PerformSearch();
                e.Handled = true; // Prevent the beep sound
            }
        }

        //RESET DATAGRIDVIEW IF TEXTBOX IS EMPTY
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text))
            {
                PerformSearch();
            }
        }

        //INSERT BUTTON
        //Home
        private void button1_Click(object sender, EventArgs e)
        {
            Insert dlg = new Insert();
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                //Refresh DataGridView when "Insert Dailog" is close
                DisplayData();
            }
        }

        //UPDATE BUTTON
        //Home
        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                // Get the data from the selected row
                DataGridViewRow row = dataGridView1.SelectedRows[0];
                int id = (int)row.Cells["Id"].Value;
                string name = row.Cells["name"].Value.ToString();
                int price = (int)row.Cells["price"].Value;
                int stock = (int)row.Cells["stock"].Value;
                int unit = (int)row.Cells["unit"].Value;
                string category = row.Cells["category"].Value.ToString();

                // Pass the data to EditDailog
                Edit dlg = new Edit(id, name, price, stock, unit, category);
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    //Refresh DataGridView when "Edit Dailog" is close
                    DisplayData();
                }
            }
        }

        //DELETE BUTTON
        //Home
        private void button3_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                int id = (int)dataGridView1.SelectedRows[0].Cells["ID"].Value;

                if (MessageBox.Show("Are you sure want to delete this item?", "Warning!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    con.Open();

                    // Construct the DELETE statement
                    SqlCommand cmd = con.CreateCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "DELETE FROM Product WHERE ID = @ID";
                    cmd.Parameters.AddWithValue("@ID", id);

                    // Execute the DELETE statement
                    cmd.ExecuteNonQuery();
                    con.Close();

                    // Remove the row from the DataGridView
                    dataGridView1.Rows.RemoveAt(dataGridView1.SelectedRows[0].Index);

                    MessageBox.Show("Record Deleted");
                }
            }
            else
            {
                MessageBox.Show("Please select a row to delete.", "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        //ADD STOCKS BUTTON
        //Home
        private void button4_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                string name = dataGridView1.SelectedRows[0].Cells["name"].Value.ToString();
                InsertStock dlg = new InsertStock(name);
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    DisplayData();
                }
            }
        }

        //HISTORY BUTTON
        //Home
        private void button5_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                int id = (int)dataGridView1.SelectedRows[0].Cells["Id"].Value;
                History historyForm = new History(id);
                historyForm.ShowDialog();
            }
        }
    }
}
