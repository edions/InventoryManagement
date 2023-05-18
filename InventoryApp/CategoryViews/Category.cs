using System.Data.SqlClient;
using System.Data;
using System.Windows.Forms;

namespace InventoryApp.InventoryApp.Views
{
    public partial class Category : Form
    {
        readonly SqlConnection con = ConnectionManager.GetConnection();
        public Category()
        {
            InitializeComponent();
            CategoryDisplay();
        }

        //FETCH DATA FROM CATEGORY DATABASE
        private void CategoryDisplay()
        {
            con.Open();
            SqlCommand cmd = con.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "select * from Category";
            cmd.ExecuteNonQuery();
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            dataGridView1.DataSource = dt;
            con.Close();
        }

        //ADD BUTTON
        //Category
        private void button1_Click(object sender, System.EventArgs e)
        {
            InsertCat dlg = new InsertCat();
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                //Refresh DataGridView when "Insert Dailog" is close
                CategoryDisplay();
            }
        }

        //UPDATE BUTTON
        //Category
        private void button2_Click(object sender, System.EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                // Get the data from the selected row
                DataGridViewRow row = dataGridView1.SelectedRows[0];
                int id = (int)row.Cells["Id"].Value;
                string categoryItem = row.Cells["CategoryItem"].Value.ToString();

                // Pass the data to EditDialog
                EditCat dlg = new EditCat(id, categoryItem);
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    // Refresh DataGridView when "EditCategory Dialog" is closed
                    CategoryDisplay();
                }
            }

        }

        //DELETE BUTTON
        //Category
        private void button3_Click(object sender, System.EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                int id = (int)dataGridView1.SelectedRows[0].Cells["ID"].Value;

                if (MessageBox.Show("Are you sure want to remove this category?", "Warning!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    con.Open();

                    // Construct the DELETE statement
                    SqlCommand cmd = con.CreateCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "DELETE FROM Category WHERE ID = @ID";
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
                MessageBox.Show("Please select a row to remove.", "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
