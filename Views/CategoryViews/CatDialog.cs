using InventoryApp.Entity;
using System;
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

        // SAVE or UPDATE BUTTON - Cat
        private void button1_Click(object sender, EventArgs e)
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

        // CANCEL BUTTON - Cat
        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
