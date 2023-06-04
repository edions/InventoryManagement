using System.Data;
using System.Windows.Forms;
using System;
using InventoryApp.Entity;

namespace InventoryApp.InventoryApp.Views
{
    public partial class Cart : Form
    {
        private readonly CartManager cartManager;
        public Cart()
        {
            InitializeComponent();
            cartManager = new CartManager();
            DisplayCartItem();
        }

        //FETCH DATA FROM CATEGORY DATABASE
        private void DisplayCartItem()
        {
            DataTable dt = cartManager.GetCartItems();
            dataGridView1.DataSource = dt;
        }

        //CHECKOUT BUTTON - Cart
        private void button1_Click(object sender, EventArgs e)
        {
            decimal totalPrice = cartManager.GetTotalPrice();
            if (totalPrice > 0)
            {
                Checkout dlg = new Checkout(totalPrice);
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    DisplayCartItem();
                }
            }
            else
            {
                MessageBox.Show("Cart is empty.", "Empty Cart", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        //ADD QUANTITY BYTTON - Cart
        private void button4_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count > 0 && dataGridView1.SelectedRows.Count > 0)
            {
                int quantity = (int)dataGridView1.SelectedRows[0].Cells["Quantity"].Value;
                int productId = (int)dataGridView1.SelectedRows[0].Cells["ProductId"].Value;

                Quantity dlg = new Quantity(quantity, productId);
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    DisplayCartItem();
                }
            }
            else
            {
                MessageBox.Show("Cart is empty.", "Empty Cart", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        //REMOVE BUTTON - Cart
        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                int id = (int)dataGridView1.SelectedRows[0].Cells["ID"].Value;

                if (MessageBox.Show("Are you sure want to remove this item from your cart?", "Warning!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    cartManager.RemoveCartItem(id);
                    DisplayCartItem();
                }
            }
            else
            {
                MessageBox.Show("Cart is empty.", "Empty Cart", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
