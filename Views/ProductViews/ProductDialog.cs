using System;
using InventoryApp.Managers;
using System.Windows.Forms;

namespace InventoryApp
{
    public partial class ProductDialog : Form
    {
        private readonly ProductManager productManager;
        private readonly int itemId; // Used for Edit mode

        public ProductDialog(ProductManager manager)
        {
            InitializeComponent();
            productManager = manager;

            // ComboBox Item
            comboBox1.Items.AddRange(productManager.GetCategoryItems());

            // Set form title for Create mode
            Text = "Add New Product";
        }

        // Constructor for Edit mode
        public ProductDialog(ProductManager manager, int id, string name, int price, int stock, int unit, string category)
        {
            InitializeComponent();
            productManager = manager;
            itemId = id;

            textBox1.Text = name;
            textBox2.Text = price.ToString();
            textBox3.Text = stock.ToString();
            textBox4.Text = unit.ToString();
            comboBox1.Text = category;

            // ComboBox Items
            comboBox1.Items.AddRange(productManager.GetCategoryItems());

            // Set form title for Edit mode
            Text = "Edit Product";
        }

        // SAVE or UPDATE BUTTON
        private void button1_Click(object sender, EventArgs e)
        {
            string selectedItem = comboBox1.Text.Trim();
            if (!string.IsNullOrEmpty(selectedItem) && comboBox1.SelectedIndex == -1)
            {
                productManager.InsertCategory(selectedItem);
            }

            if (itemId == 0) // Create mode
            {
                productManager.InsertProduct(textBox1.Text, Convert.ToInt32(textBox2.Text), Convert.ToInt32(textBox3.Text), Convert.ToInt32(textBox4.Text), comboBox1.Text);
            }
            else // Edit mode
            {
                productManager.UpdateProduct(itemId, textBox1.Text, Convert.ToInt32(textBox2.Text), Convert.ToInt32(textBox3.Text), Convert.ToInt32(textBox4.Text), comboBox1.Text);
            }

            DialogResult = DialogResult.OK;
            Close();
        }

        // CANCEL BUTTON
        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
