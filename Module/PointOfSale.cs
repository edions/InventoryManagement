using System;
using System.Windows.Forms;
using InventoryApp.Managers;
using System.Collections.Generic;

namespace InventoryApp.Services
{
    public class PointOfSale
    {
        private static readonly HashSet<string> generatedIds = new HashSet<string>();

        public void InitializeComboBox(ComboBox comboBox)
        {
            comboBox.Items.Add(new ComboBoxItem { Value = 10, Description = "10% off" });
            comboBox.Items.Add(new ComboBoxItem { Value = 15, Description = "15% off" });
            comboBox.Items.Add(new ComboBoxItem { Value = 30, Description = "30% off" });
            comboBox.Items.Add(new ComboBoxItem { Value = 50, Description = "50% off" });
            //comboBox.Items.Add("Custom");
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

            if (decimal.TryParse(paidTextBox.Text, out decimal paidAmount))
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
                return false;
            }

            double change = cash - total;
            DateTime currentDate = DateTime.Now;

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
