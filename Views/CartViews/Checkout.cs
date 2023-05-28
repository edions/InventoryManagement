using InventoryApp.Services;
using System;
using System.Windows.Forms;

namespace InventoryApp
{
    public partial class Checkout : Form
    {
        private readonly string transactionId;
        private readonly PointOfSale pointOfSale;
        public Checkout(decimal totalPrice)
        {
            InitializeComponent();
            pointOfSale = new PointOfSale();
            label3.Text = totalPrice.ToString();
            pointOfSale.InitializeComboBox(comboBox1);
            pointOfSale.CalculateDiscount(label3.Text, comboBox1.SelectedItem, label7, label8);
            pointOfSale.LoadCartItems(listBox1);

            // Attach the SelectedIndexChanged event handler
            comboBox1.SelectedIndexChanged += comboBox1_SelectedIndexChanged;

            // Attach the TextChanged event handler to textBox2
            //textBox2.TextChanged += textBox2_TextChanged;
        }

        //COMBOBOX EVENT
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            pointOfSale.CalculateDiscount(label3.Text, comboBox1.SelectedItem, label7, label8);
        }

        //TEXTBOX EVENT
        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            decimal paidAmount;
            if (decimal.TryParse(textBox2.Text, out paidAmount))
            {
                pointOfSale.CalculateChange(label8, textBox2, label10);
            }
        }

        //INSERT STOCK BUTTON
        private void button1_Click(object sender, EventArgs e)
        {
            if (pointOfSale.ProcessTransaction(label8.Text, textBox2.Text, comboBox1.SelectedItem, transactionId))
            {
                pointOfSale.InsertTransactionItems(listBox1, transactionId);
                DialogResult = DialogResult.OK;
            }
        }

        //CANCEL BUTTON
        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
