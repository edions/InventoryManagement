﻿using InventoryApp.Services;
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
        }

        // ON TEXT CHANGED
        public void ChangeEventHandler()
        {
            decimal paidAmount;
            if (decimal.TryParse(textBox2.Text, out paidAmount))
            {
                pointOfSale.CalculateChange(label8, textBox2, label10);
            }
        }

        // COMBOBOX EVENT
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            pointOfSale.CalculateDiscount(label3.Text, comboBox1.SelectedItem, label7, label8);
        }

        // TEXTBOX EVENT
        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            ChangeEventHandler();
        }

        // LABEL8 EVENT
        private void label8_TextChanged(object sender, EventArgs e)
        {
            ChangeEventHandler();
        }

        // INSERT STOCK BUTTON
        private void button1_Click(object sender, EventArgs e)
        {
            if (pointOfSale.ProcessTransaction(label3.Text, textBox2.Text, comboBox1.SelectedItem, transactionId))
            {
                pointOfSale.InsertTransactionItems(listBox1, transactionId);
                DialogResult = DialogResult.OK;
            }
        }

        // CANCEL BUTTON
        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
