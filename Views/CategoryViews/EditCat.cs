using InventoryApp.Entity;
using System;
using System.Windows.Forms;

namespace InventoryApp
{
    public partial class EditCat : Form
    {
        private readonly CategoryManager categoryManager;
        private readonly int itemId;
        public EditCat(CategoryManager manager, int id, string categoryItem)
        {
            InitializeComponent();
            categoryManager = manager;
            itemId = id;
            textBox2.Text = categoryItem;
        }

        //UPDATE BUTTON - Cat
        private void button1_Click(object sender, EventArgs e)
        {
            categoryManager.UpdateCategory(itemId, textBox2.Text);
            DialogResult = DialogResult.OK;
            Close();
        }

        //CANCEL BUTTON - Cat
        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
