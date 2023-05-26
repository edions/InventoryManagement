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
            AddToCart();
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

        //INSERT BUTTON - Home
        private void button1_Click(object sender, EventArgs e)
        {
            Insert dlg = new Insert();
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                //Refresh DataGridView when "Insert Dailog" is close
                DisplayData();
            }
        }

        //UPDATE BUTTON - Home
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
            else
            {
                MessageBox.Show("No product is available for editing.", "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        //DELETE BUTTON - Home
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
                }
            }
            else
            {
                MessageBox.Show("Please select a product to delete.", "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        //ADD STOCKS BUTTON - Home
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
            else
            {
                MessageBox.Show("No products are available for adding stock.", "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        //HISTORY BUTTON - Home
        private void button5_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                int id = (int)dataGridView1.SelectedRows[0].Cells["Id"].Value;
                History historyForm = new History(id);
                historyForm.ShowDialog();
            }
            else
            {
                MessageBox.Show("No product history is available.", "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        //ADD_TO_CART DATAGRID BUTTON - Home
        private void AddToCart()
        {
            DataGridViewButtonColumn buttonColumn = new DataGridViewButtonColumn
            {
                Text = "Add to Cart",
                UseColumnTextForButtonValue = true
            };
            dataGridView1.Columns.Add(buttonColumn);
            dataGridView1.CellContentClick -= dataGridView1_CellContentClick;
            dataGridView1.CellContentClick += dataGridView1_CellContentClick;
        }

        //DATAGRIDVIEW BUTTON EVENT - Home
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.Columns[e.ColumnIndex] is DataGridViewButtonColumn && e.RowIndex >= 0)
            {
                // Get the values from the selected row
                string name = dataGridView1.Rows[e.RowIndex].Cells["Name"].Value.ToString();
                int price = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["Price"].Value);

                // Check if the item already exists in the cart
                using (SqlConnection con = ConnectionManager.GetConnection())
                {
                    con.Open();
                    string selectQuery = "SELECT Quantity, Price FROM Cart WHERE Name = @Name";

                    using (SqlCommand selectCommand = new SqlCommand(selectQuery, con))
                    {
                        selectCommand.Parameters.AddWithValue("@Name", name);
                        SqlDataReader reader = selectCommand.ExecuteReader();

                        if (reader.Read())
                        {
                            // Item already exists in the cart, update the quantity and price
                            int existingQuantity = Convert.ToInt32(reader["Quantity"]);
                            int existingPrice = Convert.ToInt32(reader["Price"]);
                            int newQuantity = existingQuantity + 1;
                            int newPrice = existingPrice;

                            reader.Close();

                            string updateQuery = "UPDATE Cart SET Quantity = @Quantity, Price = @Price WHERE Name = @Name";

                            using (SqlCommand updateCommand = new SqlCommand(updateQuery, con))
                            {
                                updateCommand.Parameters.AddWithValue("@Quantity", newQuantity);
                                updateCommand.Parameters.AddWithValue("@Price", newPrice);
                                updateCommand.Parameters.AddWithValue("@Name", name);
                                updateCommand.ExecuteNonQuery();
                            }
                        }
                        else
                        {
                            reader.Close();

                            // Item does not exist in the cart, insert a new row
                            string insertQuery = "INSERT INTO Cart (Name, Price, Quantity) VALUES (@Name, @Price, 1)";

                            using (SqlCommand insertCommand = new SqlCommand(insertQuery, con))
                            {
                                insertCommand.Parameters.AddWithValue("@Name", name);
                                insertCommand.Parameters.AddWithValue("@Price", price);
                                insertCommand.ExecuteNonQuery();
                            }
                        }
                    }

                    con.Close();
                }

                // Provide feedback when item successfully added to cart
                MessageBox.Show("Product added to cart.");
            }
        }
    }
}
