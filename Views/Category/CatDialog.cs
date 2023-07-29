using System;
using InventoryApp.Data;
using System.Windows.Forms;

namespace InventoryApp
{
    public partial class CatDialog : Form
    {
        private readonly CategoryManager categoryManager;
        private readonly int itemId;

        public CatDialog(CategoryManager manager)
        {
            InitializeComponent();
            categoryManager = manager;

            Text = "Add New Category";
        }

        // Constructor for Edit mode - Cat
        public CatDialog(CategoryManager manager, int id, string categoryItem)
        {
            InitializeComponent();
            categoryManager = manager;
            itemId = id;
            textBox2.Text = categoryItem;

            Text = "Edit Category";
        }

        // Save Category and Validate
        private void SaveCategory()
        {
            if (!string.IsNullOrEmpty(textBox2.Text))
            {
                if (itemId == 0) // Create mode
                {
                    categoryManager.AddCategory(textBox2.Text);
                }
                else // Edit mode
                {
                    categoryManager.UpdateCategory(itemId, textBox2.Text);
                }

                DialogResult = DialogResult.OK;
                Close();
            }
            else
            {
                errorProvider1.SetError(textBox2, "Category name is required.");
            }
        }

        // SAVE or UPDATE BUTTON - Cat
        private void button1_Click(object sender, EventArgs e)
        {
            SaveCategory();
        }

        // [ ENTER ] Keypress to save
        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                SaveCategory();
                e.Handled = true;
            }
        }

        // CANCEL BUTTON - Cat
        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
