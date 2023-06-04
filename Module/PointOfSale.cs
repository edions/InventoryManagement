using InventoryApp.Managers;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace InventoryApp.Services
{
    public class PointOfSale
    {
        private readonly SqlConnection con = ConnectionManager.GetConnection();
        private static readonly HashSet<string> generatedIds = new HashSet<string>();

        public void InitializeComboBox(ComboBox comboBox)
        {
            comboBox.Items.Add(new ComboBoxItem { Value = 10, Description = "10% off" });
            comboBox.Items.Add(new ComboBoxItem { Value = 15, Description = "15% off" });
            comboBox.Items.Add(new ComboBoxItem { Value = 30, Description = "30% off" });
            comboBox.Items.Add(new ComboBoxItem { Value = 50, Description = "50% off" });
            //comboBox.Items.Add("Custom");
        }

        public void LoadCartItems(ListBox listBox)
        {
            try
            {
                con.Open();

                string selectQuery = "SELECT Name, Price, Quantity FROM Cart";

                using (SqlCommand command = new SqlCommand(selectQuery, con))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        listBox.Items.Clear();

                        while (reader.Read())
                        {
                            string name = reader["Name"].ToString();
                            int price = Convert.ToInt32(reader["Price"]);
                            int quantity = Convert.ToInt32(reader["Quantity"]);

                            string item = $"{quantity} x {name} - ${price}";
                            listBox.Items.Add(item);
                        }
                    }
                }

                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while loading cart items: " + ex.Message);
            }
        }

        public void CalculateDiscount(string totalText, object selectedItem, Label labelDiscount, Label labelTotalAfterDiscount)
        {
            int total = Convert.ToInt32(totalText);
            double discountAmount = 0;

            if (selectedItem is ComboBoxItem selectedComboBoxItem)
            {
                double discountPercent = selectedComboBoxItem.Value;
                discountAmount = total * (discountPercent / 100);
            }

            double totalAfterDiscount = total - discountAmount;
            labelDiscount.Text = (0 - discountAmount).ToString();
            labelTotalAfterDiscount.Text = totalAfterDiscount.ToString();
        }

        public void CalculateChange(Label totalLabel, TextBox paidTextBox, Label changeLabel)
        {
            decimal totalAmount = decimal.Parse(totalLabel.Text);
            decimal paidAmount;

            if (decimal.TryParse(paidTextBox.Text, out paidAmount))
            {
                decimal change = paidAmount - totalAmount;
                if (change < 0)
                {
                    change = 0;
                    changeLabel.Text = "0";
                }
                changeLabel.Text = change.ToString();
            }
            else
            {
                changeLabel.Text = string.Empty;
            }
        }

        public bool ProcessTransaction(string totalText, string cashText, object selectedItem, string transactionId)
        {
            int subtotal = Convert.ToInt32(totalText);
            int cash = string.IsNullOrWhiteSpace(cashText) ? 0 : Convert.ToInt32(cashText);
            double discountPercent = 0;
            if (selectedItem is ComboBoxItem selectedComboBoxItem)
            {
                discountPercent = selectedComboBoxItem.Value;
            }

            // Calculate the discount amount
            double discountAmount = subtotal * (discountPercent / 100);

            // Calculate the total after discount
            double total = subtotal - discountAmount;

            double totalAfterDiscount;
            // Validate if there is enough cash
            if (cash < total)
            {
                MessageBox.Show("Not enough cash to complete the transaction.");
                totalAfterDiscount = 0; // Assign 0 to totalAfterDiscount since the transaction cannot be completed
                return false;
            }

            double change = cash - total;
            DateTime currentDate = DateTime.Now;

            //SqlConnection con = ConnectionManager.GetConnection(); // Get the connection object
            TransactionManager transactionManager = new TransactionManager();
            try
            {
                transactionManager.SaveTransactionToDatabase(transactionId, subtotal, cash, discountPercent, discountAmount, change, currentDate, total);
                transactionManager.DeleteCartData();

                totalAfterDiscount = total; // Assign the calculated total after discount
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while saving the transaction: " + ex.Message);
                totalAfterDiscount = 0; // Assign 0 to totalAfterDiscount since the transaction could not be saved
                return false;
            }
        }

        public string GenerateTransactionId()
        {
            string uniqueTransactionId;

            do
            {
                uniqueTransactionId = Guid.NewGuid().ToString();
            }
            while (generatedIds.Contains(uniqueTransactionId));

            generatedIds.Add(uniqueTransactionId);

            return uniqueTransactionId;
        }
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
}
