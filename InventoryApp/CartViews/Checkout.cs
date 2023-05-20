using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace InventoryApp
{
    public partial class Checkout : Form
    {
        readonly SqlConnection con = ConnectionManager.GetConnection();
        public Checkout(int totalPrice)
        {
            InitializeComponent();
            label3.Text = totalPrice.ToString();
        }

        //INSERT STOCK BUTTON
        private void button1_Click(object sender, EventArgs e)
        {
            int total = Convert.ToInt32(label3.Text);
            int cash = string.IsNullOrWhiteSpace(textBox2.Text) ? 0 : Convert.ToInt32(textBox2.Text);
            double discountPercent = string.IsNullOrWhiteSpace(textBox1.Text) ? 0 : Convert.ToDouble(textBox1.Text);

            // Calculate the discount amount
            double discountAmount = total * (discountPercent / 100);

            // Validate if there is enough cash
            if (cash < (total - discountAmount))
            {
                MessageBox.Show("Not enough cash to complete the transaction.");
                return;
            }

            // Calculate the change
            double change = cash - (total - discountAmount);

            // Save the transaction data to the database
            try
            {
                using (SqlConnection connection = ConnectionManager.GetConnection())
                {
                    connection.Open();

                    string insertQuery = "INSERT INTO [Transaction] (Total, Cash, DiscountPercent, DiscountAmount, [Change]) VALUES (@Total, @Cash, @DiscountPercent, @DiscountAmount, @Change)";

                    using (SqlCommand command = new SqlCommand(insertQuery, connection))
                    {
                        command.Parameters.AddWithValue("@Total", total);
                        command.Parameters.AddWithValue("@Cash", cash);
                        command.Parameters.AddWithValue("@DiscountPercent", discountPercent);
                        command.Parameters.AddWithValue("@DiscountAmount", discountAmount);
                        command.Parameters.AddWithValue("@Change", change);

                        command.ExecuteNonQuery();
                    }

                    // Delete the data from the Cart table
                    string deleteQuery = "DELETE FROM [Cart]";
                    using (SqlCommand deleteCommand = new SqlCommand(deleteQuery, connection))
                    {
                        deleteCommand.ExecuteNonQuery();
                    }

                    connection.Close();
                }

                MessageBox.Show("Transaction saved successfully. Cart data deleted.");

                // Clear the input fields
                textBox2.Text = "";
                textBox1.Text = "";
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while saving the transaction: " + ex.Message);
            }
            DialogResult = DialogResult.OK;
        }

        //CANCEL BUTTON
        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
