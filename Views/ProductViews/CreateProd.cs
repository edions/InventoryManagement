using InventoryApp.Entity;
using System;
using System.Windows.Forms;

namespace InventoryApp
{
    public partial class CreateProd : Form
    {
        private readonly ProductManager productManager;

        public CreateProd(ProductManager manager)
        {
            InitializeComponent();
            productManager = manager;

            //ComboBox Item
            comboBox1.Items.AddRange(productManager.GetCategoryItems());
        }

        //SAVE BUTTON
        private void button1_Click(object sender, EventArgs e)
        {
            string selectedItem = comboBox1.Text.Trim();
            if (!string.IsNullOrEmpty(selectedItem) && comboBox1.SelectedIndex == -1)
            {
                productManager.InsertCategory(selectedItem);
            }

            productManager.InsertProduct(textBox1.Text, Convert.ToInt32(textBox2.Text), Convert.ToInt32(textBox3.Text), Convert.ToInt32(textBox4.Text), comboBox1.Text);

            DialogResult = DialogResult.OK;
            Close();
        }

        //CANCEL BUTTON
        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
