using InventoryApp.Entity;
using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace InventoryApp
{
    public partial class EditProd : Form
    {
        private readonly ProductManager productManager;
        readonly private int itemId;

        public EditProd(ProductManager manager, int id, string name, int price, int stock, int unit, string category)
        {
            InitializeComponent();
            productManager = manager;
            itemId = id;

            textBox1.Text = name;
            textBox2.Text = price.ToString();
            textBox3.Text = stock.ToString();
            textBox4.Text = unit.ToString();
            comboBox1.Text = category;

            //ComboBox Item
            comboBox1.Items.AddRange(productManager.GetCategoryItems());
        }

        //UPDATE BUTTON
        private void button1_Click(object sender, EventArgs e)
        {
            string selectedItem = comboBox1.Text.Trim();
            if (!string.IsNullOrEmpty(selectedItem) && comboBox1.SelectedIndex == -1)
            {
                productManager.InsertCategory(selectedItem);
            }

            productManager.UpdateProduct(itemId, textBox1.Text, Convert.ToInt32(textBox2.Text), Convert.ToInt32(textBox3.Text), Convert.ToInt32(textBox4.Text), comboBox1.Text);

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
