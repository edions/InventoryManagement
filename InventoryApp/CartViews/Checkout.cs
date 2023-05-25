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
            InitializeComboBox(comboBox1);

            // Attach the SelectedIndexChanged event handler
            comboBox1.SelectedIndexChanged += comboBox1_SelectedIndexChanged;
        }

        public static void InitializeComboBox(ComboBox comboBox)
        {
            comboBox.Items.Add(new ComboBoxItem { Value = 10, Description = "10% off" });
            comboBox.Items.Add(new ComboBoxItem { Value = 15, Description = "15% off" });
            comboBox.Items.Add(new ComboBoxItem { Value = 30, Description = "30% off" });
            comboBox.Items.Add(new ComboBoxItem { Value = 50, Description = "50% off" });

        }

        public class ComboBoxItem
        {
            public double Value { get; set; }
            public string Description { get; set; }

            public override string ToString()
            {
                return Description;
            }
        }

        private void CalculateDiscount()
        {
            int total = Convert.ToInt32(label3.Text);
            double discountAmount = 0;

            if (comboBox1.SelectedItem is ComboBoxItem selectedItem)
            {
                double discountPercent = selectedItem.Value;

                // Calculate the discount amount
                discountAmount = total * (discountPercent / 100);
            }

            // Display the discount amount in label7
            label7.Text = (0 - discountAmount).ToString();
        }


        //INSERT STOCK BUTTON
        private void button1_Click(object sender, EventArgs e)
        {
            int total = Convert.ToInt32(label3.Text);
            int cash = string.IsNullOrWhiteSpace(textBox2.Text) ? 0 : Convert.ToInt32(textBox2.Text);
            double discountPercent = 0;
            if (comboBox1.SelectedItem is ComboBoxItem selectedItem)
            {
                discountPercent = selectedItem.Value;
            }

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
                con.Open();
                string insertQuery = "INSERT INTO [Transaction] (Total, Cash, DiscountPercent, DiscountAmount, [Change]) VALUES (@Total, @Cash, @DiscountPercent, @DiscountAmount, @Change)";

                using (SqlCommand command = new SqlCommand(insertQuery, con))
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
                using (SqlCommand deleteCommand = new SqlCommand(deleteQuery, con))
                {
                    deleteCommand.ExecuteNonQuery();
                }
                con.Close();
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

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            CalculateDiscount();
        }
    }
}
