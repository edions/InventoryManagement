using System;
using System.Data;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace InventoryApp
{
    public partial class Main : Form
    {
        readonly SqlConnection con = ConnectionManager.GetConnection();
        public Main()
        {
            InitializeComponent();
            DisplayData();
        }

        //FETCH DATA FROM DATABASE
        public void DisplayData()
        {
            con.Open();
            SqlCommand cmd = con.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "select * from Inventory";
            cmd.ExecuteNonQuery();
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            dataGridView1.DataSource = dt;
            con.Close();
        }

        //INSERT BUTTON
        private void button1_Click(object sender, EventArgs e)
        {
            InsertDialog dlg = new InsertDialog();
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                //Refresh DataGridView when "Insert Dailog" is close
                DisplayData();
            }
        }

        //UPDATE BUTTON
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
                EditDialog dlg = new EditDialog(id, name, price, stock, unit, category);
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    //Refresh DataGridView when "Edit Dailog" is close
                    DisplayData();
                }
            }
        }

        //DELETE BUTTON
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
                    cmd.CommandText = "DELETE FROM Inventory WHERE ID = @ID";
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
    }
}
