using System;
using InventoryApp.Data;
using System.Windows.Forms;

namespace InventoryApp
{
    public partial class Quantity : Form
    {
        private readonly CartManager cartManager;

        private readonly int productId;
        public Quantity(int quantity, int productId)
        {
            InitializeComponent();

            this.productId = productId;
            textBox2.Text = quantity.ToString();

            cartManager = new CartManager();
        }

        //MINUS BUTTON
        private void button3_Click(object sender, EventArgs e)
        {
            DecrementQuantity();
        }

        //PLUS BUTTON
        private void button4_Click(object sender, EventArgs e)
        {
            IncrementQuantity();
        }

        //SAVE BUTTON
        private void button1_Click(object sender, EventArgs e)
        {
            UpdateQuantityInCart();
            DialogResult = DialogResult.OK;
            Close();
        }

        //CANCEL BUTTON
        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void DecrementQuantity()
        {
            if (int.TryParse(textBox2.Text, out int value))
            {
                if (value > 1)
                {
                    value--;
                    textBox2.Text = value.ToString();
                }
            }
        }

        private void IncrementQuantity()
        {
            StockManager stockManager = new StockManager();

            int value = int.Parse(textBox2.Text);
            int stock = stockManager.GetProductStock(productId);
            if (value < stock)
            {
                value++;
                textBox2.Text = value.ToString();
            } else
            {
                MessageBox.Show("Stock limit reached.");
            }
        }

        private void UpdateQuantityInCart()
        {
            string quantity = textBox2.Text;
            cartManager.UpdateQuantityInCart(productId, quantity);
        }
    }
}
